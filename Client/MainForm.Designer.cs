namespace Client
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._serverAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._serverPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._buttonStart = new System.Windows.Forms.Button();
            this._buttonStop = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this._login = new System.Windows.Forms.TextBox();
            this._buttonLogin = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this._transport = new System.Windows.Forms.ComboBox();
            this._buttonSend = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this._message = new System.Windows.Forms.TextBox();
            this._buttonClear = new System.Windows.Forms.Button();
            this._messages = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // _serverAddress
            // 
            this._serverAddress.Location = new System.Drawing.Point(15, 27);
            this._serverAddress.Name = "_serverAddress";
            this._serverAddress.Size = new System.Drawing.Size(152, 20);
            this._serverAddress.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Address:";
            // 
            // _serverPort
            // 
            this._serverPort.Location = new System.Drawing.Point(173, 27);
            this._serverPort.Name = "_serverPort";
            this._serverPort.Size = new System.Drawing.Size(65, 20);
            this._serverPort.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(170, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Port:";
            // 
            // _buttonStart
            // 
            this._buttonStart.Location = new System.Drawing.Point(381, 26);
            this._buttonStart.Name = "_buttonStart";
            this._buttonStart.Size = new System.Drawing.Size(65, 23);
            this._buttonStart.TabIndex = 4;
            this._buttonStart.Text = "Start";
            this._buttonStart.UseVisualStyleBackColor = true;
            this._buttonStart.Click += new System.EventHandler(this.HandleButtonStartClick);
            // 
            // _buttonStop
            // 
            this._buttonStop.Location = new System.Drawing.Point(452, 26);
            this._buttonStop.Name = "_buttonStop";
            this._buttonStop.Size = new System.Drawing.Size(65, 23);
            this._buttonStop.TabIndex = 5;
            this._buttonStop.Text = "Stop";
            this._buttonStop.UseVisualStyleBackColor = true;
            this._buttonStop.Click += new System.EventHandler(this.HandleButtonStopClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Login:";
            // 
            // _login
            // 
            this._login.Location = new System.Drawing.Point(15, 80);
            this._login.Name = "_login";
            this._login.Size = new System.Drawing.Size(152, 20);
            this._login.TabIndex = 6;
            // 
            // _buttonLogin
            // 
            this._buttonLogin.Location = new System.Drawing.Point(173, 78);
            this._buttonLogin.Name = "_buttonLogin";
            this._buttonLogin.Size = new System.Drawing.Size(65, 23);
            this._buttonLogin.TabIndex = 8;
            this._buttonLogin.Text = "Login";
            this._buttonLogin.UseVisualStyleBackColor = true;
            this._buttonLogin.Click += new System.EventHandler(this.HandleButtonLoginClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(241, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Transport:";
            // 
            // _transport
            // 
            this._transport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._transport.FormattingEnabled = true;
            this._transport.Location = new System.Drawing.Point(244, 27);
            this._transport.Name = "_transport";
            this._transport.Size = new System.Drawing.Size(131, 21);
            this._transport.TabIndex = 10;
            // 
            // _buttonSend
            // 
            this._buttonSend.Location = new System.Drawing.Point(381, 137);
            this._buttonSend.Name = "_buttonSend";
            this._buttonSend.Size = new System.Drawing.Size(65, 23);
            this._buttonSend.TabIndex = 13;
            this._buttonSend.Text = "Send";
            this._buttonSend.UseVisualStyleBackColor = true;
            this._buttonSend.Click += new System.EventHandler(this.HandleButtonSendClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Message:";
            // 
            // _message
            // 
            this._message.Location = new System.Drawing.Point(15, 139);
            this._message.Name = "_message";
            this._message.Size = new System.Drawing.Size(360, 20);
            this._message.TabIndex = 11;
            // 
            // _buttonClear
            // 
            this._buttonClear.Location = new System.Drawing.Point(452, 137);
            this._buttonClear.Name = "_buttonClear";
            this._buttonClear.Size = new System.Drawing.Size(65, 23);
            this._buttonClear.TabIndex = 14;
            this._buttonClear.Text = "Clear";
            this._buttonClear.UseVisualStyleBackColor = true;
            this._buttonClear.Click += new System.EventHandler(this.HandleButtonClearClick);
            // 
            // _messages
            // 
            this._messages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._messages.FormattingEnabled = true;
            this._messages.Location = new System.Drawing.Point(15, 166);
            this._messages.Name = "_messages";
            this._messages.Size = new System.Drawing.Size(502, 277);
            this._messages.TabIndex = 15;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 450);
            this.Controls.Add(this._messages);
            this.Controls.Add(this._buttonClear);
            this.Controls.Add(this._buttonSend);
            this.Controls.Add(this.label5);
            this.Controls.Add(this._message);
            this.Controls.Add(this._transport);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._buttonLogin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._login);
            this.Controls.Add(this._buttonStop);
            this.Controls.Add(this._buttonStart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._serverPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._serverAddress);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _serverAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _serverPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button _buttonStart;
        private System.Windows.Forms.Button _buttonStop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _login;
        private System.Windows.Forms.Button _buttonLogin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox _transport;
        private System.Windows.Forms.Button _buttonSend;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox _message;
        private System.Windows.Forms.Button _buttonClear;
        private System.Windows.Forms.ListBox _messages;
    }
}

