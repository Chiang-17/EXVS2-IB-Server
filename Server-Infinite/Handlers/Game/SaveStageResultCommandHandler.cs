using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerInfinite.Models.Cards;
using ServerInfinite.Models.Cards.Battle;
using ServerInfinite.Models.Cards.Settings;
using ServerInfinite.Models.Config;
using ServerInfinite.Persistence;

namespace ServerInfinite.Handlers.Game;

public record SaveStageResultCommand(Request Request) : IRequest<Response>;

/// <summary>
/// Route battle stage summary (vsac29).
/// - Updates best clear time (min, non-zero) / best clear score (max) for stage 1..3 of the difficulty.
/// - On final_stage_clear_flag updates total best time / score.
/// - save_data_info present  -> upsert "continue" save data (current_gauge = after_gauge).
///   save_data_info absent   -> route finished or abandoned, delete save data.
/// Badge (route level) up/down is decided by the game client; badge exp is stored in SaveVscResult.
/// </summary>
public class SaveStageResultCommandHandler : IRequestHandler<SaveStageResultCommand, Response>
{
    private readonly ServerDbContext _context;
    private readonly ILogger<SaveStageResultCommandHandler> _logger;
    private readonly CardServerConfig _config;

    public SaveStageResultCommandHandler(ServerDbContext context, ILogger<SaveStageResultCommandHandler> logger, IOptions<CardServerConfig> options)
    {
        _context = context;
        _logger = logger;
        _config = options.Value;
    }

    public Task<Response> Handle(SaveStageResultCommand request, CancellationToken cancellationToken)
    {
        var saveRequest = request.Request.save_stage_result;
        if (saveRequest == null)
        {
            return Task.FromResult(SuccessResponse(request));
        }

        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.SessionId == saveRequest.SessionId && x.Id == saveRequest.PilotId);

        var result = saveRequest.Result;
        if (cardProfile == null || result == null)
        {
            _logger.LogWarning("SaveStageResult: card not found or empty result. PilotId={PilotId}", saveRequest.PilotId);
            return Task.FromResult(SuccessResponse(request));
        }

        _logger.LogInformation("SaveStageResult CardId={CardId} difficulty={Difficulty} stage={Stage} clear={Clear} time={Time} score={Score} gauge={Before}->{After} final={Final} total={TotalTime}/{TotalScore} save_data={HasSave}",
            cardProfile.Id, result.Difficulty, result.StageNum, result.StageClearFlag,
            result.ShouldSerializeStageClearTime() ? result.StageClearTime : 0u,
            result.ShouldSerializeStageClearScore() ? result.StageClearScore : 0u,
            result.BeforeGauge, result.AfterGauge, result.FinalStageClearFlag,
            result.TotalClearTime, result.TotalClearScore, result.save_data_info != null);

        SaveHighScore(cardProfile, result);
        SaveContinueData(cardProfile, result);
        _context.SaveChanges();

        return Task.FromResult(SuccessResponse(request));
    }

    private void SaveHighScore(CardProfile cardProfile, Request.SaveStageResult.PlayResultGroup result)
    {
        var hasStage = result.StageClearFlag && result.StageNum >= 1 && result.StageNum <= 3;
        var hasTotal = result.FinalStageClearFlag && (result.ShouldSerializeTotalClearTime() || result.ShouldSerializeTotalClearScore());
        if (!hasStage && !hasTotal)
        {
            return;
        }

        var cardId = cardProfile.Id;
        var difficulty = result.Difficulty;
        var row = _context.RouteBattleHighScoreDbSet.FirstOrDefault(x => x.CardId == cardId && x.Difficulty == difficulty);
        if (row == null)
        {
            row = new RouteBattleHighScore
            {
                CardId = cardId,
                Difficulty = difficulty
            };
            _context.RouteBattleHighScoreDbSet.Add(row);
        }

        if (hasStage)
        {
            var time = result.ShouldSerializeStageClearTime() ? result.StageClearTime : 0u;
            var score = result.ShouldSerializeStageClearScore() ? result.StageClearScore : result.Score;
            switch (result.StageNum)
            {
                case 1u:
                    row.ClearTime1 = BestTime(row.ClearTime1, time);
                    row.ClearScore1 = Math.Max(row.ClearScore1, score);
                    break;
                case 2u:
                    row.ClearTime2 = BestTime(row.ClearTime2, time);
                    row.ClearScore2 = Math.Max(row.ClearScore2, score);
                    break;
                case 3u:
                    row.ClearTime3 = BestTime(row.ClearTime3, time);
                    row.ClearScore3 = Math.Max(row.ClearScore3, score);
                    break;
            }
        }

        if (hasTotal)
        {
            if (result.ShouldSerializeTotalClearTime())
            {
                row.TotalClearTime = BestTime(row.TotalClearTime, result.TotalClearTime);
            }
            if (result.ShouldSerializeTotalClearScore())
            {
                row.TotalClearScore = Math.Max(row.TotalClearScore, result.TotalClearScore);
            }
        }
    }

    /// <summary>Smaller is better; 0 means "no record".</summary>
    private static uint BestTime(uint current, uint incoming)
    {
        if (incoming == 0)
        {
            return current;
        }
        if (current == 0)
        {
            return incoming;
        }
        return Math.Min(current, incoming);
    }

    private void SaveContinueData(CardProfile cardProfile, Request.SaveStageResult.PlayResultGroup result)
    {
        var cardId = cardProfile.Id;
        var data = _context.StageSaveDataDbSet.FirstOrDefault(x => x.CardId == cardId);
        var info = result.save_data_info;
        if (info == null)
        {
            if (data != null)
            {
                _context.StageSaveDataDbSet.Remove(data);
            }
            return;
        }

        if (data == null)
        {
            data = new StageSaveData
            {
                CardId = cardId
            };
            _context.StageSaveDataDbSet.Add(data);
        }

        data.PatternId = info.PatternId;
        data.Difficulty = info.Difficulty;
        data.StageNum = info.StageNum;
        data.BossId1 = info.BossId1;
        data.BossId2 = info.ShouldSerializeBossId2() ? info.BossId2 : null;
        data.MaxGauge = info.MaxGauge;
        data.CurrentGauge = result.AfterGauge;
        data.MstMobileSuitId = result.MstMobileSuitId;
        data.CustomChipIds = string.Join(",", info.CustomChipIds ?? Array.Empty<uint>());
        data.SaveDataUrl = _config.GameConfigurations.RouteBattleConfigurations?.SaveDataUrl ?? string.Empty;
    }

    private static Response SuccessResponse(SaveStageResultCommand request) =>
        new()
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_stage_result = new Response.SaveStageResult()
        };
}
