using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubVV
{
    public class SubsProfile
    {
        public string remark { get; set; }
        public string url { get; set; }
        public List<Vmess> vmessList;
        public List<SubsProfile> subsRefList;

        public SubsProfile()
        {
            url = "";
            remark = "";
            vmessList = new List<Vmess>();
            subsRefList = new List<SubsProfile>();
        }

        public SubsProfile(string url, string remark = "")
        {
            this.url = url;
            this.remark = remark;
            vmessList = new List<Vmess>();
            subsRefList = new List<SubsProfile>();
        }
    }
}
