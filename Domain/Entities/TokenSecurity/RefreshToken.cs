using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.TokenSecurity;
//yeni yaratdim 
public class RefreshToken
{

    public int?  Id { get; set; } 
    public string Token { get; set; }
    public int UserId { get; set; }
    public DateTime ExpiryDate { get; set; }    

}
