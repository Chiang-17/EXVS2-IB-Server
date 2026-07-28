using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerInfinite.Mapper.Card.Sticker;
using ServerInfinite.Models.Cards.MobileSuit;
using ServerInfinite.Persistence;
using WebUIInfinite.Shared.Dto.Common;
using WebUIInfinite.Shared.Dto.Request.Sticker;
using WebUIInfinite.Shared.Dto.Response;

namespace ServerInfinite.Handlers.UI.Sticker;

public record UpsertMobileSuitStickersCommand(UpsertMobileSuitStickersRequest Request) : IRequest<BasicResponse>;

public class UpsertMobileSuitStickersCommandHandler : IRequestHandler<UpsertMobileSuitStickersCommand, BasicResponse>
{
    private readonly ServerDbContext _context;

    public UpsertMobileSuitStickersCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpsertMobileSuitStickersCommand request, CancellationToken cancellationToken)
    {
        var upsertRequest = request.Request;
        
        var response = new BasicResponse
        {
            Success = false
        };

        var cardProfile = _context.CardProfiles
            .Include(x => x.Navis)
            .FirstOrDefault(x => x.AccessCode == upsertRequest.AccessCode && x.ChipId == upsertRequest.ChipId);

        if (cardProfile == null)
        {
            return Task.FromResult(response);
        }
        
        var stickerDtos = upsertRequest.MsStickerList;
        
        stickerDtos.ForEach(stickerDto =>
        {
            StickerPilotPoolDefaults.ApplyEmptyPoolDefaultsOnSave(stickerDto);

            var existingSticker = _context.MobileSuitStickerDbSet
                .FirstOrDefault(x => x.CardProfile == cardProfile && x.MstMobileSuitId == stickerDto.MobileSuitId);

            if (existingSticker is null)
            {
                var poseIds = stickerDto.PoseIds ?? new List<uint>();

                _context.Add(new MobileSuitSticker()
                {
                    CardProfile = cardProfile,
                    MstMobileSuitId = stickerDto.MobileSuitId,
                    PoseId = poseIds.FirstOrDefault(),
                    PoseIds = StickerDtoMapping.ToCsv(poseIds),
                    CostumeIds = StickerDtoMapping.ToCsv(stickerDto.CostumeIds),
                    PilotImageDispFlag = stickerDto.PilotImageDispFlag,
                    CutinImageUseFlag = stickerDto.PilotImageDispFlag && stickerDto.CutinImageUseFlag,
                    StickerBackgroundId = stickerDto.StickerBackgroundId,
                    SubBackgroundId = stickerDto.SubBackgroundId,
                    StickerEffectId = stickerDto.StickerEffectId,
                    Tracker1 = stickerDto.Tracker1,
                    Tracker2 = stickerDto.Tracker2,
                    Tracker3 = stickerDto.Tracker3,
                });
                
                _context.SaveChanges();
                return;
            }

            var existingPoseIds = stickerDto.PoseIds ?? new List<uint>();

            existingSticker.PoseId = existingPoseIds.FirstOrDefault();
            existingSticker.PoseIds = StickerDtoMapping.ToCsv(existingPoseIds);
            existingSticker.CostumeIds = StickerDtoMapping.ToCsv(stickerDto.CostumeIds);
            existingSticker.PilotImageDispFlag = stickerDto.PilotImageDispFlag;
            existingSticker.CutinImageUseFlag = stickerDto.PilotImageDispFlag && stickerDto.CutinImageUseFlag;

            existingSticker.StickerBackgroundId = stickerDto.StickerBackgroundId;
            existingSticker.SubBackgroundId = stickerDto.SubBackgroundId;
            existingSticker.StickerEffectId = stickerDto.StickerEffectId;
            
            existingSticker.Tracker1 = stickerDto.Tracker1;
            existingSticker.Tracker2 = stickerDto.Tracker2;
            existingSticker.Tracker3 = stickerDto.Tracker3;
        });
        
        _context.SaveChanges();

        response.Success = true;

        return Task.FromResult(response);
    }
}