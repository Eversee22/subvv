namespace SubVV
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonRemove = new System.Windows.Forms.Button();
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxUserId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxAlterId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxLevel = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxMethod = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxRemark = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxNet = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxHost = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxTls = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxServItems = new System.Windows.Forms.ListBox();
            this.groupBoxServerInfo = new System.Windows.Forms.GroupBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.listBoxSubs = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.textBoxUrlIn = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBoxServerInfo.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(175, 305);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 8;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // textBoxServer
            // 
            this.textBoxServer.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxServer.Location = new System.Drawing.Point(82, 22);
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.ReadOnly = true;
            this.textBoxServer.Size = new System.Drawing.Size(193, 21);
            this.textBoxServer.TabIndex = 0;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(297, 22);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.ReadOnly = true;
            this.textBoxPort.Size = new System.Drawing.Size(76, 21);
            this.textBoxPort.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(281, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = ":";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Address:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "UserId:";
            // 
            // textBoxUserId
            // 
            this.textBoxUserId.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxUserId.Location = new System.Drawing.Point(82, 54);
            this.textBoxUserId.Name = "textBoxUserId";
            this.textBoxUserId.ReadOnly = true;
            this.textBoxUserId.Size = new System.Drawing.Size(291, 21);
            this.textBoxUserId.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "AlterId:";
            // 
            // textBoxAlterId
            // 
            this.textBoxAlterId.Location = new System.Drawing.Point(82, 92);
            this.textBoxAlterId.Name = "textBoxAlterId";
            this.textBoxAlterId.ReadOnly = true;
            this.textBoxAlterId.Size = new System.Drawing.Size(84, 21);
            this.textBoxAlterId.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(223, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "Level:";
            // 
            // textBoxLevel
            // 
            this.textBoxLevel.Location = new System.Drawing.Point(283, 92);
            this.textBoxLevel.Name = "textBoxLevel";
            this.textBoxLevel.ReadOnly = true;
            this.textBoxLevel.Size = new System.Drawing.Size(90, 21);
            this.textBoxLevel.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 134);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "Method:";
            // 
            // textBoxMethod
            // 
            this.textBoxMethod.Location = new System.Drawing.Point(82, 128);
            this.textBoxMethod.Name = "textBoxMethod";
            this.textBoxMethod.ReadOnly = true;
            this.textBoxMethod.Size = new System.Drawing.Size(211, 21);
            this.textBoxMethod.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 238);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "Remark:";
            // 
            // textBoxRemark
            // 
            this.textBoxRemark.Location = new System.Drawing.Point(82, 235);
            this.textBoxRemark.Name = "textBoxRemark";
            this.textBoxRemark.ReadOnly = true;
            this.textBoxRemark.Size = new System.Drawing.Size(211, 21);
            this.textBoxRemark.TabIndex = 23;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 170);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 15;
            this.label9.Text = "Net:";
            // 
            // textBoxNet
            // 
            this.textBoxNet.Location = new System.Drawing.Point(82, 167);
            this.textBoxNet.Name = "textBoxNet";
            this.textBoxNet.ReadOnly = true;
            this.textBoxNet.Size = new System.Drawing.Size(76, 21);
            this.textBoxNet.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(172, 170);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 12);
            this.label10.TabIndex = 17;
            this.label10.Text = "Path:";
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(213, 167);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.ReadOnly = true;
            this.textBoxPath.Size = new System.Drawing.Size(160, 21);
            this.textBoxPath.TabIndex = 18;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 203);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 12);
            this.label11.TabIndex = 19;
            this.label11.Text = "Host:";
            // 
            // textBoxHost
            // 
            this.textBoxHost.Location = new System.Drawing.Point(82, 203);
            this.textBoxHost.Name = "textBoxHost";
            this.textBoxHost.ReadOnly = true;
            this.textBoxHost.Size = new System.Drawing.Size(159, 21);
            this.textBoxHost.TabIndex = 20;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(262, 208);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 21;
            this.label12.Text = "TLS:";
            // 
            // textBoxTls
            // 
            this.textBoxTls.Location = new System.Drawing.Point(297, 203);
            this.textBoxTls.Name = "textBoxTls";
            this.textBoxTls.ReadOnly = true;
            this.textBoxTls.Size = new System.Drawing.Size(76, 21);
            this.textBoxTls.TabIndex = 22;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxServItems);
            this.groupBox1.Controls.Add(this.groupBoxServerInfo);
            this.groupBox1.Location = new System.Drawing.Point(28, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(513, 328);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server List";
            // 
            // listBoxServItems
            // 
            this.listBoxServItems.FormattingEnabled = true;
            this.listBoxServItems.HorizontalScrollbar = true;
            this.listBoxServItems.ItemHeight = 12;
            this.listBoxServItems.Location = new System.Drawing.Point(4, 35);
            this.listBoxServItems.Name = "listBoxServItems";
            this.listBoxServItems.Size = new System.Drawing.Size(118, 268);
            this.listBoxServItems.TabIndex = 6;
            this.listBoxServItems.SelectedIndexChanged += new System.EventHandler(this.listBoxServItems_SelectedIndexChanged);
            // 
            // groupBoxServerInfo
            // 
            this.groupBoxServerInfo.Controls.Add(this.textBoxTls);
            this.groupBoxServerInfo.Controls.Add(this.label12);
            this.groupBoxServerInfo.Controls.Add(this.textBoxHost);
            this.groupBoxServerInfo.Controls.Add(this.label11);
            this.groupBoxServerInfo.Controls.Add(this.textBoxPath);
            this.groupBoxServerInfo.Controls.Add(this.label10);
            this.groupBoxServerInfo.Controls.Add(this.textBoxNet);
            this.groupBoxServerInfo.Controls.Add(this.label9);
            this.groupBoxServerInfo.Controls.Add(this.textBoxRemark);
            this.groupBoxServerInfo.Controls.Add(this.label8);
            this.groupBoxServerInfo.Controls.Add(this.textBoxMethod);
            this.groupBoxServerInfo.Controls.Add(this.label7);
            this.groupBoxServerInfo.Controls.Add(this.textBoxLevel);
            this.groupBoxServerInfo.Controls.Add(this.label6);
            this.groupBoxServerInfo.Controls.Add(this.textBoxAlterId);
            this.groupBoxServerInfo.Controls.Add(this.label5);
            this.groupBoxServerInfo.Controls.Add(this.textBoxUserId);
            this.groupBoxServerInfo.Controls.Add(this.label4);
            this.groupBoxServerInfo.Controls.Add(this.label2);
            this.groupBoxServerInfo.Controls.Add(this.label3);
            this.groupBoxServerInfo.Controls.Add(this.textBoxPort);
            this.groupBoxServerInfo.Controls.Add(this.textBoxServer);
            this.groupBoxServerInfo.Location = new System.Drawing.Point(128, 35);
            this.groupBoxServerInfo.Name = "groupBoxServerInfo";
            this.groupBoxServerInfo.Size = new System.Drawing.Size(380, 274);
            this.groupBoxServerInfo.TabIndex = 7;
            this.groupBoxServerInfo.TabStop = false;
            this.groupBoxServerInfo.Text = "Server Information";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(530, 22);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(59, 23);
            this.buttonAdd.TabIndex = 5;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // listBoxSubs
            // 
            this.listBoxSubs.FormattingEnabled = true;
            this.listBoxSubs.HorizontalScrollbar = true;
            this.listBoxSubs.ItemHeight = 12;
            this.listBoxSubs.Location = new System.Drawing.Point(11, 23);
            this.listBoxSubs.Name = "listBoxSubs";
            this.listBoxSubs.Size = new System.Drawing.Size(239, 268);
            this.listBoxSubs.TabIndex = 4;
            this.listBoxSubs.SelectedIndexChanged += new System.EventHandler(this.listBoxSubs_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "URL:";
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(81, 305);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 0;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // textBoxUrlIn
            // 
            this.textBoxUrlIn.Location = new System.Drawing.Point(100, 22);
            this.textBoxUrlIn.Name = "textBoxUrlIn";
            this.textBoxUrlIn.Size = new System.Drawing.Size(414, 21);
            this.textBoxUrlIn.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBoxSubs);
            this.groupBox2.Controls.Add(this.buttonUpdate);
            this.groupBox2.Controls.Add(this.buttonRemove);
            this.groupBox2.Location = new System.Drawing.Point(549, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(261, 328);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Subscription List";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(28, 419);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 9;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(724, 419);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 11;
            this.buttonClose.Text = "Exit";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 444);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxUrlIn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "SubVV";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBoxServerInfo.ResumeLayout(false);
            this.groupBoxServerInfo.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxUserId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxAlterId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxLevel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxMethod;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxRemark;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxNet;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxHost;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxTls;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBoxServItems;
        private System.Windows.Forms.GroupBox groupBoxServerInfo;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ListBox listBoxSubs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.TextBox textBoxUrlIn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonClose;
    }
}

