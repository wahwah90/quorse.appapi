using Quorse.EntityFramework.EntityFramework.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.DAL.Repositories.Base
{
    public class BaseFuncRepository
    {
        private QuorseDbEntities db = new QuorseDbEntities();
        
        public BaseFuncRepository() {  }

        //convert to myr from usd
        protected decimal? usdToRM(decimal? myr, bool? isUsd,decimal? usdExchangeRate)
        {
            var _myr = myr;
            if (isUsd==true)
            {
                _myr = myr* usdExchangeRate;
               
            }
            return _myr;
        }
        //get lowest price->ttprice,promoprice,listprice
        protected decimal? lowestClassPrice(classDetail cd, decimal? usdExchangeRate)
        {
            decimal? lowestPrice = null;

            //lowest to highest ->ttprice,promoprice,listprice
            if (cd.ttprice != null && cd.ttprice > 0)
            {
                lowestPrice = cd.ttprice;
            }
            else if (cd.promoprice != null && cd.promoprice > 0)
            {
                lowestPrice = cd.promoprice;
            }
            else
            {
                lowestPrice = cd.price;
            }

            //check price is in usd
            lowestPrice = usdToRM(lowestPrice, cd.isusd,usdExchangeRate);
            return lowestPrice;
        }
    }
}
