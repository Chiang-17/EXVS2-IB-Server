using nue.protocol.exvs;
using ServerInfinite.Models.Cards;
using ServerInfinite.Persistence;

namespace ServerInfinite.Commands.LoadCard.PilotData;

/// <summary>LoadCard.pilot_data_group.high_score_info (tag 6): route battle best records per difficulty.</summary>
public class RouteBattleHighScoreCommand : IPilotDataCommand
{
    private readonly ServerDbContext _context;

    public RouteBattleHighScoreCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.PilotDataGroup pilotDataGroup)
    {
        var cardId = cardProfile.Id;
        var rows = _context.RouteBattleHighScoreDbSet
            .Where(x => x.CardId == cardId)
            .OrderBy(x => x.Difficulty)
            .ToList();

        foreach (var r in rows)
        {
            pilotDataGroup.HighScoreInfos.Add(new Response.LoadCard.PilotDataGroup.HighScoreInfo
            {
                Difficulty = r.Difficulty,
                ClearTime1 = r.ClearTime1,
                ClearScore1 = r.ClearScore1,
                ClearTime2 = r.ClearTime2,
                ClearScore2 = r.ClearScore2,
                ClearTime3 = r.ClearTime3,
                ClearScore3 = r.ClearScore3,
                TotalClearTime = r.TotalClearTime,
                TotalClearScore = r.TotalClearScore
            });
        }
    }
}
