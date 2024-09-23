using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Abstract
{
    public interface IValidator<T>
    {
        string ErrorMessage { get; set; }

        ////1.stringe Name , 2.stringe hata mesajı olucak şekilde Dictionary yaparak da kullanılabilir
        //Dictionary<string,string> Validate(T entity);

        bool Validate(T entity);
    }
}
