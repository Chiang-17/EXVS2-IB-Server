using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerInfinite.Models.Cards.Titles.User;

[Table("exvs2ob_user_sub_title_1")]
[Index(nameof(Id))]
[Index(nameof(UserTitleSettingId))]
public class UserSubTitle1 : UserTitle
{
}
