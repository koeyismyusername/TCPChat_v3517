namespace TCPChatProject_3517
{
    partial class ChatClientForm
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
            this.labelRoomId = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.numRoomId = new System.Windows.Forms.NumericUpDown();
            this.tBoxUsername = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.lBoxMessages = new System.Windows.Forms.ListBox();
            this.tBoxMessage = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numRoomId)).BeginInit();
            this.SuspendLayout();
            // 
            // labelRoomId
            // 
            this.labelRoomId.AutoSize = true;
            this.labelRoomId.Location = new System.Drawing.Point(12, 14);
            this.labelRoomId.Name = "labelRoomId";
            this.labelRoomId.Size = new System.Drawing.Size(48, 12);
            this.labelRoomId.TabIndex = 0;
            this.labelRoomId.Text = "RoomId";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(12, 42);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(63, 12);
            this.labelUsername.TabIndex = 1;
            this.labelUsername.Text = "Username";
            // 
            // numRoomId
            // 
            this.numRoomId.Location = new System.Drawing.Point(81, 12);
            this.numRoomId.Name = "numRoomId";
            this.numRoomId.Size = new System.Drawing.Size(120, 21);
            this.numRoomId.TabIndex = 2;
            this.numRoomId.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tBoxUsername
            // 
            this.tBoxUsername.Location = new System.Drawing.Point(81, 39);
            this.tBoxUsername.Name = "tBoxUsername";
            this.tBoxUsername.Size = new System.Drawing.Size(120, 21);
            this.tBoxUsername.TabIndex = 3;
            this.tBoxUsername.Text = "짱구";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(230, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 48);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "연결하기";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(311, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 48);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "종료하기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(311, 301);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "전송하기";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lBoxMessages
            // 
            this.lBoxMessages.FormattingEnabled = true;
            this.lBoxMessages.ItemHeight = 12;
            this.lBoxMessages.Location = new System.Drawing.Point(12, 77);
            this.lBoxMessages.Name = "lBoxMessages";
            this.lBoxMessages.Size = new System.Drawing.Size(374, 220);
            this.lBoxMessages.TabIndex = 7;
            // 
            // tBoxMessage
            // 
            this.tBoxMessage.Location = new System.Drawing.Point(12, 303);
            this.tBoxMessage.Name = "tBoxMessage";
            this.tBoxMessage.Size = new System.Drawing.Size(293, 21);
            this.tBoxMessage.TabIndex = 8;
            this.tBoxMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tBoxMessage_KeyDown);
            // 
            // ChatClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 336);
            this.Controls.Add(this.tBoxMessage);
            this.Controls.Add(this.lBoxMessages);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.tBoxUsername);
            this.Controls.Add(this.numRoomId);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.labelRoomId);
            this.Name = "ChatClientForm";
            this.Text = "Client";
            ((System.ComponentModel.ISupportInitialize)(this.numRoomId)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRoomId;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.NumericUpDown numRoomId;
        private System.Windows.Forms.TextBox tBoxUsername;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ListBox lBoxMessages;
        private System.Windows.Forms.TextBox tBoxMessage;
    }
}

