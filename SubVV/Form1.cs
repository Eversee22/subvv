using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Newtonsoft.Json;
using System.Threading;
using System.IO;
using System.Web.Script.Serialization;

namespace SubVV
{
    public partial class Form1 : Form
    {
        private static JavaScriptSerializer js = new JavaScriptSerializer();
        private static string subsSaveDir = "Subscriptions";
        public List<Dictionary<string, object>> profiles; // only vmess
        public List<SubsProfile> subsProfiles;

        public Form1()
        {
            InitializeComponent();
            Lang.InitControl(this);

            subsProfiles = new List<SubsProfile>();
            profiles = new List<Dictionary<string, object>>();
            SubsProfile subsProfile0 = new SubsProfile();
            string filepath = string.Format("{0}\\{1}.json", subsSaveDir, Utils.MD5Sum("-"));
            if (File.Exists(filepath))
            {
                SubsProfile temp = IOSerialize.readingJson(filepath);
                subsProfile0.subsRefList = temp.subsRefList;
            }
            subsProfiles.Add(subsProfile0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBoxSubs.Items.Add("-all-");
            foreach (SubsProfile subsProfile in subsProfiles[0].subsRefList)
            {
                subsProfiles.Add(subsProfile);
                listBoxSubs.Items.Add(subsProfile.url);
            }
            //MessageBox.Show("loaded " + (subsProfiles.Count - 1));
            listBoxSubs.SelectedIndex = subsProfiles.Count - 1;
        }

        private void updateUrl()
        {
            try
            {
                BackgroundWorker subscribeWorker = new BackgroundWorker();
                subscribeWorker.WorkerSupportsCancellation = true;
                subscribeWorker.DoWork += subscribeWorker_DoWork;
                if (subscribeWorker.IsBusy)
                    return;
                subscribeWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "request timeout");
                return;
            }
        }

        private void updateUrl(SubsProfile subsProfile)
        {
            try
            {
                BackgroundWorker subscribeWorker = new BackgroundWorker();
                subscribeWorker.WorkerSupportsCancellation = true;
                subscribeWorker.DoWork += subscribeWorker_DoWork2;
                if (subscribeWorker.IsBusy)
                    return;
                subscribeWorker.RunWorkerAsync(subsProfile);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "request timeout");
                return;
            }
        }

        public void RefreshListServBox()
        {
            var selectedIndex = listBoxServItems.SelectedIndex;
            listBoxServItems.Items.Clear();
            foreach (Dictionary<string, object> profile in profiles)
            {
                listBoxServItems.Items.Add(profile["tag"]);
            }
            if (selectedIndex >= 0)
                listBoxServItems.SelectedIndex = Math.Min(selectedIndex, profiles.Count - 1);
            else
                listBoxServItems.SelectedIndex = profiles.Count - 1;
        }

        #region import & subscribe

        void subscribeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            profiles.Clear();
            //if (subsProfiles[0].vmessList.Count > 0)
            //    subsProfiles[0].vmessList.Clear();
        
            for (int i=1; i<subsProfiles.Count; i++)
            {
                SubsProfile item = subsProfiles[i];
                //MessageBox.Show("Update:\n" + item.url);
                if (item.vmessList.Count > 0)
                    item.vmessList.Clear();
                var tag = ImportURL(Utils.Base64Decode(Utils.GetUrl(item.url)), item);
                //foreach (Vmess v in item.vmessList)
                //    subsProfiles[0].vmessList.Add(v);
            }

            RefreshListServBox();
        }

        void subscribeWorker_DoWork2(object sender, DoWorkEventArgs e)
        {
            SubsProfile subsProfile = (SubsProfile)e.Argument;
            profiles.Clear();
            if (subsProfile.vmessList.Count > 0)
                subsProfile.vmessList.Clear();
            //MessageBox.Show("Update:\n" + subsProfile.url);
            var tag = ImportURL(Utils.Base64Decode(Utils.GetUrl(subsProfile.url)), subsProfile);
            //foreach (Vmess v in subsProfile.vmessList)
            //    subsProfiles[0].vmessList.Add(v);
            RefreshListServBox();
        }

        List<string> ImportURL(string importUrl, SubsProfile subsProfile = null)
        {
            List<string> linkMark = new List<string>();
            foreach (var link in importUrl.Split(Environment.NewLine.ToCharArray()))
            {
                //if (link.StartsWith("ss"))
                //{
                //    linkMark.Add(ImportShadowsocks(link));
                //}

                if (link.StartsWith("vmess"))
                {
                    Vmess vmess = ImportVmess(link);
                    linkMark.Add(vmess.ps);
                    if (subsProfile != null)
                    {
                        subsProfile.vmessList.Add(vmess);
                    }
                }
            }
            Debug.WriteLine("importurl " + String.Join(",", linkMark));
            return linkMark;
        }

        //public string ImportShadowsocks(string url)
        //{
        //    var link = url.Contains("#") ? url.Substring(5, url.IndexOf("#") - 5) : url.Substring(5);
        //    var tag = url.Contains("#") ? url.Substring(url.IndexOf("#") + 1).Trim() : "Newtag" + new Random(Guid.NewGuid().GetHashCode()).Next(100, 1000);
        //    var linkParseArray = Utils.Base64Decode(link).Split(new char[2] { ':', '@' });
        //    Dictionary<string, object> ShadowsocksProfiles = Utilities.outboundTemplate;
        //    Dictionary<string, object> ShadowsocksTemplate = Utilities.ShadowsocksOutboundTemplateNew();
        //    ShadowsocksProfiles["protocol"] = "shadowsocks";
        //    ShadowsocksProfiles["tag"] = tag;
        //    ShadowsocksTemplate["email"] = "love@server.cc";
        //    ShadowsocksTemplate["address"] = linkParseArray[2];
        //    ShadowsocksTemplate["port"] = Convert.ToInt32(linkParseArray[3]);
        //    ShadowsocksTemplate["method"] = linkParseArray[0];
        //    ShadowsocksTemplate["password"] = linkParseArray[1];
        //    ShadowsocksTemplate["ota"] = false;
        //    ShadowsocksTemplate["level"] = 0;
        //    ShadowsocksProfiles["settings"] = new Dictionary<string, object> {
        //            {"servers",  new List<Dictionary<string, object>>{ ShadowsocksTemplate } }
        //        };
        //    outbounds.Add(Utilities.DeepClone(ShadowsocksProfiles));
        //    return tag;
        //}

        public Vmess ImportVmess(string vmessUrl)
        {
            Vmess VmessLink = js.Deserialize<Vmess>(Utils.Base64Decode(vmessUrl.Substring(8)));

            profiles.Add(Utils.GenVmessConfig(VmessLink));
            return VmessLink;
        }

        public void ImportVmess(Vmess VmessLink)
        {
            profiles.Add(Utils.GenVmessConfig(VmessLink));
        }

        #endregion

        private void listBoxSubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedIndex = listBoxSubs.SelectedIndex;
            if (selectedIndex > 0 && selectedIndex < subsProfiles.Count)
            {
                if (subsProfiles[selectedIndex].vmessList.Count == 0)
                {
                    //profiles.Clear();
                    listBoxServItems.Items.Clear();
                    listBoxServItems.SelectedIndex = -1;
                    return;
                }
                    
                profiles.Clear();
                foreach (Vmess vmess in subsProfiles[selectedIndex].vmessList)
                    ImportVmess(vmess);
               //RefreshListServBox();
            }
            else if(selectedIndex == 0)
            {
                profiles.Clear();
                foreach (SubsProfile subsProfile in subsProfiles[0].subsRefList)
                    foreach (Vmess v in subsProfile.vmessList)
                        ImportVmess(v);
                //RefreshListServBox();
            }

            RefreshListServBox();
        }

        private void listBoxServItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxServItems.SelectedIndex >= 0 && listBoxServItems.SelectedIndex < profiles.Count)
            {
                Dictionary<string, object> selectedProfile = profiles[listBoxServItems.SelectedIndex];
                Dictionary<string, object> selectedVnext = ((selectedProfile["settings"] as Dictionary<string, object>)["vnext"] as IList<object>)[0] as Dictionary<string, object>;
                Dictionary<string, object> selectedUserInfo = (selectedVnext["users"] as IList<object>)[0] as Dictionary<string, object>;
                Dictionary<string, object> selectedStreamSetting = selectedProfile["streamSettings"] as Dictionary<string, object>;
                Dictionary<string, object> selectedWsSetting = selectedStreamSetting["wsSettings"] as Dictionary<string, object>;
                Dictionary<string, object> selectedWsSettingH = selectedWsSetting["headers"] as Dictionary<string, object>;
                textBoxServer.Text = selectedVnext["address"].ToString();
                textBoxPort.Text = selectedVnext["port"].ToString();
                textBoxUserId.Text = selectedUserInfo["id"].ToString();
                textBoxAlterId.Text = selectedUserInfo["alterId"].ToString();
                textBoxMethod.Text = selectedUserInfo["security"].ToString();
                textBoxLevel.Text = selectedUserInfo["level"].ToString();
                textBoxRemark.Text = selectedProfile["tag"].ToString();
                textBoxNet.Text = selectedStreamSetting["network"].ToString();
                textBoxPath.Text = selectedWsSetting["path"].ToString();
                textBoxHost.Text = selectedWsSettingH["host"].ToString();
                textBoxTls.Text = selectedStreamSetting["security"].ToString();
            }
        }

        private void setTexBoxesDefault()
        {
            textBoxServer.Text = "yourserver.domain";
            textBoxPort.Text = "12345";
            textBoxUserId.Text = "12345678-1234-4321-1234-123456789abc";
            textBoxAlterId.Text = "0";
            textBoxMethod.Text = "none";
            textBoxLevel.Text = "0";
            textBoxRemark.Text = "default";
            textBoxNet.Text = "tcp";
            textBoxPath.Text = "";
            textBoxHost.Text = "";
            textBoxTls.Text = "";
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string url = textBoxUrlIn.Text;
            if (url.Length > 0)
            {
                SubsProfile subsProfile = new SubsProfile(url);
                subsProfiles.Add(subsProfile);
                listBoxSubs.Items.Add(url);
                listBoxSubs.SelectedIndex = subsProfiles.Count - 1;
                subsProfiles[0].subsRefList.Add(subsProfile);
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (listBoxSubs.Items.Count == 1)
            {
                setTexBoxesDefault();
                return;
            }
                
            //this.url = textBoxUrlIn.Text;
            var selectedIndex = listBoxSubs.SelectedIndex;
            //MessageBox.Show("selectedIndex " + selectedIndex);
            if (selectedIndex > 0 && selectedIndex < subsProfiles.Count)
            {
                updateUrl(subsProfiles[selectedIndex]);
                //SubsProfile subsProfile = subsProfiles[selectedIndex];
                //profiles.Clear();
                //ImportURL(Utils.Base64Decode(Utils.GetUrl(subsProfile.url)), subsProfile);
                //MessageBox.Show("update over");
                //RefreshListServBox();
            }
            else if (selectedIndex == 0)
            {
                updateUrl();
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            var selectedIndex = listBoxSubs.SelectedIndex;
            if (selectedIndex > 0 && selectedIndex < subsProfiles.Count)
            {
                listBoxSubs.Items.RemoveAt(selectedIndex);
                SubsProfile deletedSubs = subsProfiles[selectedIndex];
                subsProfiles.Remove(deletedSubs);
                subsProfiles[0].subsRefList.Remove(deletedSubs);
                deletedSubs.vmessList.Clear();
                if (listBoxSubs.Items.Count == 1)
                {
                    setTexBoxesDefault();
                }
                listBoxSubs.SelectedIndex = subsProfiles.Count - 1;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveAll();
            MessageBox.Show("All saved");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            saveAll();
            this.Close();
        }

        private void saveAll()
        {
            if (!Directory.Exists(subsSaveDir))
            {
                Directory.CreateDirectory(subsSaveDir);
            }

            IOSerialize.writingJson(subsProfiles[0], subsSaveDir);
        }
    }
}
