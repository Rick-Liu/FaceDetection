namespace FaceDetection
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.BtnDetect = new System.Windows.Forms.Button();
            this.BtnInPut = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(18, 18);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1254, 894);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(1386, 74);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(248, 246);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // BtnDetect
            // 
            this.BtnDetect.Location = new System.Drawing.Point(1428, 494);
            this.BtnDetect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnDetect.Name = "BtnDetect";
            this.BtnDetect.Size = new System.Drawing.Size(158, 54);
            this.BtnDetect.TabIndex = 3;
            this.BtnDetect.Text = "Detect";
            this.BtnDetect.UseVisualStyleBackColor = true;
            this.BtnDetect.Click += new System.EventHandler(this.BtnDetect_Click);
            // 
            // BtnInPut
            // 
            this.BtnInPut.Location = new System.Drawing.Point(1428, 392);
            this.BtnInPut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnInPut.Name = "BtnInPut";
            this.BtnInPut.Size = new System.Drawing.Size(158, 54);
            this.BtnInPut.TabIndex = 4;
            this.BtnInPut.Text = "InPut";
            this.BtnInPut.UseVisualStyleBackColor = true;
            this.BtnInPut.Click += new System.EventHandler(this.BtnInPut_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1386, 654);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(264, 128);
            this.button1.TabIndex = 5;
            this.button1.Text = "Detect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1776, 1204);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnInPut);
            this.Controls.Add(this.BtnDetect);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button BtnDetect;
        private System.Windows.Forms.Button BtnInPut;
        private System.Windows.Forms.Button button1;
    }
}

