namespace TCPChatServer
{
    partial class ChatServerForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lBoxClients = new System.Windows.Forms.ListBox();
            this.lBoxMessages = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(374, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "시작하기";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(455, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "종료하기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lBoxClients
            // 
            this.lBoxClients.FormattingEnabled = true;
            this.lBoxClients.ItemHeight = 12;
            this.lBoxClients.Location = new System.Drawing.Point(12, 42);
            this.lBoxClients.Name = "lBoxClients";
            this.lBoxClients.Size = new System.Drawing.Size(198, 292);
            this.lBoxClients.TabIndex = 2;
            // 
            // lBoxMessages
            // 
            this.lBoxMessages.FormattingEnabled = true;
            this.lBoxMessages.ItemHeight = 12;
            this.lBoxMessages.Location = new System.Drawing.Point(216, 42);
            this.lBoxMessages.Name = "lBoxMessages";
            this.lBoxMessages.Size = new System.Drawing.Size(314, 292);
            this.lBoxMessages.TabIndex = 3;
            // 
            // ChatServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 346);
            this.Controls.Add(this.lBoxMessages);
            this.Controls.Add(this.lBoxClients);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStart);
            this.Name = "ChatServerForm";
            this.Text = "Server";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ListBox lBoxClients;
        private System.Windows.Forms.ListBox lBoxMessages;
    }
}

