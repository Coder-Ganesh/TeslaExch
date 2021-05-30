using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Probet247.Models
{
    public class AutoTPSPC
    {
        betfairbEntities auto_tp = new betfairbEntities();
        public List<AutoTp_Result> GetTPBets()
        {
            return auto_tp.AutoTp().ToList();
        }

    }
}