<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Options
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Options))
        Me.TabControl_Options = New System.Windows.Forms.TabControl()
        Me.TabPage_General = New System.Windows.Forms.TabPage()
        Me.Label_GEN_9 = New System.Windows.Forms.Label()
        Me.Label_GEN_19 = New System.Windows.Forms.Label()
        Me.TextBox_GEN_ProcessName = New System.Windows.Forms.TextBox()
        Me.Button_GEN_ResetGeneralSettings = New System.Windows.Forms.Button()
        Me.Label_GEN_5 = New System.Windows.Forms.Label()
        Me.TextBox_GEN_Filepath = New System.Windows.Forms.TextBox()
        Me.Button_GEN_SelectDB = New System.Windows.Forms.Button()
        Me.TabPage_OCR = New System.Windows.Forms.TabPage()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox_OCR_PV = New System.Windows.Forms.TextBox()
        Me.Label_OCR_5 = New System.Windows.Forms.Label()
        Me.Label_OCR_15 = New System.Windows.Forms.Label()
        Me.CheckBox_OCR_DelScrsht = New System.Windows.Forms.CheckBox()
        Me.Label_OCR_14 = New System.Windows.Forms.Label()
        Me.Label_OCR_13 = New System.Windows.Forms.Label()
        Me.TextBox_OCR_BlackTH = New System.Windows.Forms.TextBox()
        Me.TextBox_OCR_Grayscale = New System.Windows.Forms.TextBox()
        Me.Label_OCR_12 = New System.Windows.Forms.Label()
        Me.Label_OCR_11 = New System.Windows.Forms.Label()
        Me.Label_OCR_6 = New System.Windows.Forms.Label()
        Me.Label_OCR_4 = New System.Windows.Forms.Label()
        Me.Label_OCR_3 = New System.Windows.Forms.Label()
        Me.Label_OCR_2 = New System.Windows.Forms.Label()
        Me.Label_OCR_1 = New System.Windows.Forms.Label()
        Me.TextBox_OCR_Lang = New System.Windows.Forms.TextBox()
        Me.Label_OCR_23 = New System.Windows.Forms.Label()
        Me.Label_OCR_21 = New System.Windows.Forms.Label()
        Me.Label_OCR_20 = New System.Windows.Forms.Label()
        Me.TextBox_OCR_Gamma = New System.Windows.Forms.TextBox()
        Me.TextBox_OCR_Contrast = New System.Windows.Forms.TextBox()
        Me.TextBox_OCR_Confidence = New System.Windows.Forms.TextBox()
        Me.Label_OCR_18 = New System.Windows.Forms.Label()
        Me.NUD_OCR_Scaling = New System.Windows.Forms.NumericUpDown()
        Me.Label_OCR_10 = New System.Windows.Forms.Label()
        Me.Button_OCR_ResetOCRSettings = New System.Windows.Forms.Button()
        Me.Button_OCR_TempFolder = New System.Windows.Forms.Button()
        Me.TextBox_OCR_TempFolder = New System.Windows.Forms.TextBox()
        Me.Label_OCR_8 = New System.Windows.Forms.Label()
        Me.Button_OCR_TessdataFolder = New System.Windows.Forms.Button()
        Me.TextBox_OCR_Tessdata = New System.Windows.Forms.TextBox()
        Me.Label_OCR_7 = New System.Windows.Forms.Label()
        Me.TabPage_Visibility = New System.Windows.Forms.TabPage()
        Me.TabControl_Visibility = New System.Windows.Forms.TabControl()
        Me.TabPage_VIS_Systems = New System.Windows.Forms.TabPage()
        Me.ListView_VIS_Systems = New System.Windows.Forms.ListView()
        Me.TabPage_VIS_Stations = New System.Windows.Forms.TabPage()
        Me.ListView_VIS_Stations = New System.Windows.Forms.ListView()
        Me.TabPage_VIS_Commodities = New System.Windows.Forms.TabPage()
        Me.ListView_VIS_Commodities = New System.Windows.Forms.ListView()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.Button_SaveSettings = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.TabControl_Options.SuspendLayout()
        Me.TabPage_General.SuspendLayout()
        Me.TabPage_OCR.SuspendLayout()
        CType(Me.NUD_OCR_Scaling, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage_Visibility.SuspendLayout()
        Me.TabControl_Visibility.SuspendLayout()
        Me.TabPage_VIS_Systems.SuspendLayout()
        Me.TabPage_VIS_Stations.SuspendLayout()
        Me.TabPage_VIS_Commodities.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl_Options
        '
        Me.TabControl_Options.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl_Options.Controls.Add(Me.TabPage_General)
        Me.TabControl_Options.Controls.Add(Me.TabPage_OCR)
        Me.TabControl_Options.Controls.Add(Me.TabPage_Visibility)
        Me.TabControl_Options.Location = New System.Drawing.Point(1, 0)
        Me.TabControl_Options.Name = "TabControl_Options"
        Me.TabControl_Options.SelectedIndex = 0
        Me.TabControl_Options.Size = New System.Drawing.Size(884, 266)
        Me.TabControl_Options.TabIndex = 0
        '
        'TabPage_General
        '
        Me.TabPage_General.Controls.Add(Me.Label_GEN_9)
        Me.TabPage_General.Controls.Add(Me.Label_GEN_19)
        Me.TabPage_General.Controls.Add(Me.TextBox_GEN_ProcessName)
        Me.TabPage_General.Controls.Add(Me.Button_GEN_ResetGeneralSettings)
        Me.TabPage_General.Controls.Add(Me.Label_GEN_5)
        Me.TabPage_General.Controls.Add(Me.TextBox_GEN_Filepath)
        Me.TabPage_General.Controls.Add(Me.Button_GEN_SelectDB)
        Me.TabPage_General.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_General.Name = "TabPage_General"
        Me.TabPage_General.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_General.Size = New System.Drawing.Size(876, 240)
        Me.TabPage_General.TabIndex = 0
        Me.TabPage_General.Text = "General"
        Me.TabPage_General.UseVisualStyleBackColor = True
        '
        'Label_GEN_9
        '
        Me.Label_GEN_9.AutoSize = True
        Me.Label_GEN_9.Location = New System.Drawing.Point(323, 45)
        Me.Label_GEN_9.Name = "Label_GEN_9"
        Me.Label_GEN_9.Size = New System.Drawing.Size(174, 13)
        Me.Label_GEN_9.TabIndex = 57
        Me.Label_GEN_9.Text = "(default value = 'EliteDangerous32')"
        '
        'Label_GEN_19
        '
        Me.Label_GEN_19.AutoSize = True
        Me.Label_GEN_19.Location = New System.Drawing.Point(18, 45)
        Me.Label_GEN_19.Name = "Label_GEN_19"
        Me.Label_GEN_19.Size = New System.Drawing.Size(94, 13)
        Me.Label_GEN_19.TabIndex = 51
        Me.Label_GEN_19.Text = "ED Process Name"
        '
        'TextBox_GEN_ProcessName
        '
        Me.TextBox_GEN_ProcessName.Location = New System.Drawing.Point(172, 42)
        Me.TextBox_GEN_ProcessName.Name = "TextBox_GEN_ProcessName"
        Me.TextBox_GEN_ProcessName.Size = New System.Drawing.Size(135, 20)
        Me.TextBox_GEN_ProcessName.TabIndex = 50
        Me.TextBox_GEN_ProcessName.Text = "EliteDangerous32"
        '
        'Button_GEN_ResetGeneralSettings
        '
        Me.Button_GEN_ResetGeneralSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_GEN_ResetGeneralSettings.Location = New System.Drawing.Point(8, 209)
        Me.Button_GEN_ResetGeneralSettings.Name = "Button_GEN_ResetGeneralSettings"
        Me.Button_GEN_ResetGeneralSettings.Size = New System.Drawing.Size(153, 23)
        Me.Button_GEN_ResetGeneralSettings.TabIndex = 26
        Me.Button_GEN_ResetGeneralSettings.Text = "Reset to default values"
        Me.Button_GEN_ResetGeneralSettings.UseVisualStyleBackColor = True
        '
        'Label_GEN_5
        '
        Me.Label_GEN_5.AutoSize = True
        Me.Label_GEN_5.Location = New System.Drawing.Point(18, 19)
        Me.Label_GEN_5.Name = "Label_GEN_5"
        Me.Label_GEN_5.Size = New System.Drawing.Size(135, 13)
        Me.Label_GEN_5.TabIndex = 19
        Me.Label_GEN_5.Text = "Specify Database Location"
        '
        'TextBox_GEN_Filepath
        '
        Me.TextBox_GEN_Filepath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_GEN_Filepath.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.TextBox_GEN_Filepath.Location = New System.Drawing.Point(172, 16)
        Me.TextBox_GEN_Filepath.Name = "TextBox_GEN_Filepath"
        Me.TextBox_GEN_Filepath.Size = New System.Drawing.Size(551, 20)
        Me.TextBox_GEN_Filepath.TabIndex = 20
        '
        'Button_GEN_SelectDB
        '
        Me.Button_GEN_SelectDB.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_GEN_SelectDB.Location = New System.Drawing.Point(729, 16)
        Me.Button_GEN_SelectDB.Name = "Button_GEN_SelectDB"
        Me.Button_GEN_SelectDB.Size = New System.Drawing.Size(31, 20)
        Me.Button_GEN_SelectDB.TabIndex = 21
        Me.Button_GEN_SelectDB.Text = "..."
        Me.Button_GEN_SelectDB.UseVisualStyleBackColor = True
        '
        'TabPage_OCR
        '
        Me.TabPage_OCR.Controls.Add(Me.Label1)
        Me.TabPage_OCR.Controls.Add(Me.TextBox_OCR_PV)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_5)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_15)
        Me.TabPage_OCR.Controls.Add(Me.CheckBox_OCR_DelScrsht)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_14)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_13)
        Me.TabPage_OCR.Controls.Add(Me.TextBox_OCR_BlackTH)
        Me.TabPage_OCR.Controls.Add(Me.TextBox_OCR_Grayscale)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_12)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_11)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_6)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_4)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_3)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_2)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_1)
        Me.TabPage_OCR.Controls.Add(Me.TextBox_OCR_Lang)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_23)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_21)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_20)
        Me.TabPage_OCR.Controls.Add(Me.TextBox_OCR_Gamma)
        Me.TabPage_OCR.Controls.Add(Me.TextBox_OCR_Contrast)
        Me.TabPage_OCR.Controls.Add(Me.TextBox_OCR_Confidence)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_18)
        Me.TabPage_OCR.Controls.Add(Me.NUD_OCR_Scaling)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_10)
        Me.TabPage_OCR.Controls.Add(Me.Button_OCR_ResetOCRSettings)
        Me.TabPage_OCR.Controls.Add(Me.Button_OCR_TempFolder)
        Me.TabPage_OCR.Controls.Add(Me.TextBox_OCR_TempFolder)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_8)
        Me.TabPage_OCR.Controls.Add(Me.Button_OCR_TessdataFolder)
        Me.TabPage_OCR.Controls.Add(Me.TextBox_OCR_Tessdata)
        Me.TabPage_OCR.Controls.Add(Me.Label_OCR_7)
        Me.TabPage_OCR.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_OCR.Name = "TabPage_OCR"
        Me.TabPage_OCR.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_OCR.Size = New System.Drawing.Size(876, 240)
        Me.TabPage_OCR.TabIndex = 1
        Me.TabPage_OCR.Text = "OCR"
        Me.TabPage_OCR.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(660, 145)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 13)
        Me.Label1.TabIndex = 71
        Me.Label1.Text = "(default value = 20)"
        '
        'TextBox_OCR_PV
        '
        Me.TextBox_OCR_PV.Location = New System.Drawing.Point(595, 142)
        Me.TextBox_OCR_PV.Name = "TextBox_OCR_PV"
        Me.TextBox_OCR_PV.Size = New System.Drawing.Size(47, 20)
        Me.TextBox_OCR_PV.TabIndex = 70
        Me.TextBox_OCR_PV.Text = "20"
        Me.TextBox_OCR_PV.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label_OCR_5
        '
        Me.Label_OCR_5.AutoSize = True
        Me.Label_OCR_5.Location = New System.Drawing.Point(389, 145)
        Me.Label_OCR_5.Name = "Label_OCR_5"
        Me.Label_OCR_5.Size = New System.Drawing.Size(143, 13)
        Me.Label_OCR_5.TabIndex = 69
        Me.Label_OCR_5.Text = "Price Variance Threshold (%)"
        '
        'Label_OCR_15
        '
        Me.Label_OCR_15.AutoSize = True
        Me.Label_OCR_15.Location = New System.Drawing.Point(389, 122)
        Me.Label_OCR_15.Name = "Label_OCR_15"
        Me.Label_OCR_15.Size = New System.Drawing.Size(181, 13)
        Me.Label_OCR_15.TabIndex = 68
        Me.Label_OCR_15.Text = "Delete Screenshot after DB Update?"
        '
        'CheckBox_OCR_DelScrsht
        '
        Me.CheckBox_OCR_DelScrsht.AutoSize = True
        Me.CheckBox_OCR_DelScrsht.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox_OCR_DelScrsht.Location = New System.Drawing.Point(608, 122)
        Me.CheckBox_OCR_DelScrsht.Name = "CheckBox_OCR_DelScrsht"
        Me.CheckBox_OCR_DelScrsht.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox_OCR_DelScrsht.TabIndex = 67
        Me.CheckBox_OCR_DelScrsht.UseVisualStyleBackColor = True
        '
        'Label_OCR_14
        '
        Me.Label_OCR_14.AutoSize = True
        Me.Label_OCR_14.Enabled = False
        Me.Label_OCR_14.Location = New System.Drawing.Point(237, 96)
        Me.Label_OCR_14.Name = "Label_OCR_14"
        Me.Label_OCR_14.Size = New System.Drawing.Size(98, 13)
        Me.Label_OCR_14.TabIndex = 66
        Me.Label_OCR_14.Text = "(default value = 65)"
        '
        'Label_OCR_13
        '
        Me.Label_OCR_13.AutoSize = True
        Me.Label_OCR_13.Location = New System.Drawing.Point(237, 70)
        Me.Label_OCR_13.Name = "Label_OCR_13"
        Me.Label_OCR_13.Size = New System.Drawing.Size(92, 13)
        Me.Label_OCR_13.TabIndex = 65
        Me.Label_OCR_13.Text = "(default value = 0)"
        '
        'TextBox_OCR_BlackTH
        '
        Me.TextBox_OCR_BlackTH.Enabled = False
        Me.TextBox_OCR_BlackTH.Location = New System.Drawing.Point(172, 93)
        Me.TextBox_OCR_BlackTH.Name = "TextBox_OCR_BlackTH"
        Me.TextBox_OCR_BlackTH.Size = New System.Drawing.Size(47, 20)
        Me.TextBox_OCR_BlackTH.TabIndex = 64
        Me.TextBox_OCR_BlackTH.Text = "65"
        Me.TextBox_OCR_BlackTH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox_OCR_Grayscale
        '
        Me.TextBox_OCR_Grayscale.Location = New System.Drawing.Point(172, 67)
        Me.TextBox_OCR_Grayscale.Name = "TextBox_OCR_Grayscale"
        Me.TextBox_OCR_Grayscale.Size = New System.Drawing.Size(47, 20)
        Me.TextBox_OCR_Grayscale.TabIndex = 63
        Me.TextBox_OCR_Grayscale.Text = "0"
        Me.TextBox_OCR_Grayscale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label_OCR_12
        '
        Me.Label_OCR_12.AutoSize = True
        Me.Label_OCR_12.Location = New System.Drawing.Point(17, 70)
        Me.Label_OCR_12.Name = "Label_OCR_12"
        Me.Label_OCR_12.Size = New System.Drawing.Size(81, 13)
        Me.Label_OCR_12.TabIndex = 62
        Me.Label_OCR_12.Text = "Grayscale Type"
        '
        'Label_OCR_11
        '
        Me.Label_OCR_11.AutoSize = True
        Me.Label_OCR_11.Location = New System.Drawing.Point(17, 96)
        Me.Label_OCR_11.Name = "Label_OCR_11"
        Me.Label_OCR_11.Size = New System.Drawing.Size(84, 13)
        Me.Label_OCR_11.TabIndex = 61
        Me.Label_OCR_11.Text = "Black Threshold"
        '
        'Label_OCR_6
        '
        Me.Label_OCR_6.AutoSize = True
        Me.Label_OCR_6.Location = New System.Drawing.Point(237, 170)
        Me.Label_OCR_6.Name = "Label_OCR_6"
        Me.Label_OCR_6.Size = New System.Drawing.Size(92, 13)
        Me.Label_OCR_6.TabIndex = 60
        Me.Label_OCR_6.Text = "(default value = 4)"
        '
        'Label_OCR_4
        '
        Me.Label_OCR_4.AutoSize = True
        Me.Label_OCR_4.Location = New System.Drawing.Point(237, 145)
        Me.Label_OCR_4.Name = "Label_OCR_4"
        Me.Label_OCR_4.Size = New System.Drawing.Size(98, 13)
        Me.Label_OCR_4.TabIndex = 59
        Me.Label_OCR_4.Text = "(default value = 67)"
        '
        'Label_OCR_3
        '
        Me.Label_OCR_3.AutoSize = True
        Me.Label_OCR_3.Location = New System.Drawing.Point(237, 119)
        Me.Label_OCR_3.Name = "Label_OCR_3"
        Me.Label_OCR_3.Size = New System.Drawing.Size(104, 13)
        Me.Label_OCR_3.TabIndex = 58
        Me.Label_OCR_3.Text = "(default value = 'big')"
        '
        'Label_OCR_2
        '
        Me.Label_OCR_2.AutoSize = True
        Me.Label_OCR_2.Location = New System.Drawing.Point(660, 96)
        Me.Label_OCR_2.Name = "Label_OCR_2"
        Me.Label_OCR_2.Size = New System.Drawing.Size(107, 13)
        Me.Label_OCR_2.TabIndex = 57
        Me.Label_OCR_2.Text = "(default value = 0.98)"
        '
        'Label_OCR_1
        '
        Me.Label_OCR_1.AutoSize = True
        Me.Label_OCR_1.Location = New System.Drawing.Point(660, 70)
        Me.Label_OCR_1.Name = "Label_OCR_1"
        Me.Label_OCR_1.Size = New System.Drawing.Size(98, 13)
        Me.Label_OCR_1.TabIndex = 56
        Me.Label_OCR_1.Text = "(default value = 31)"
        '
        'TextBox_OCR_Lang
        '
        Me.TextBox_OCR_Lang.Location = New System.Drawing.Point(172, 116)
        Me.TextBox_OCR_Lang.Name = "TextBox_OCR_Lang"
        Me.TextBox_OCR_Lang.Size = New System.Drawing.Size(47, 20)
        Me.TextBox_OCR_Lang.TabIndex = 55
        Me.TextBox_OCR_Lang.Text = "big"
        Me.TextBox_OCR_Lang.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label_OCR_23
        '
        Me.Label_OCR_23.AutoSize = True
        Me.Label_OCR_23.Location = New System.Drawing.Point(17, 119)
        Me.Label_OCR_23.Name = "Label_OCR_23"
        Me.Label_OCR_23.Size = New System.Drawing.Size(55, 13)
        Me.Label_OCR_23.TabIndex = 54
        Me.Label_OCR_23.Text = "Language"
        '
        'Label_OCR_21
        '
        Me.Label_OCR_21.AutoSize = True
        Me.Label_OCR_21.Location = New System.Drawing.Point(389, 96)
        Me.Label_OCR_21.Name = "Label_OCR_21"
        Me.Label_OCR_21.Size = New System.Drawing.Size(94, 13)
        Me.Label_OCR_21.TabIndex = 53
        Me.Label_OCR_21.Text = "Gamma Correction"
        '
        'Label_OCR_20
        '
        Me.Label_OCR_20.AutoSize = True
        Me.Label_OCR_20.Location = New System.Drawing.Point(389, 70)
        Me.Label_OCR_20.Name = "Label_OCR_20"
        Me.Label_OCR_20.Size = New System.Drawing.Size(97, 13)
        Me.Label_OCR_20.TabIndex = 52
        Me.Label_OCR_20.Text = "Contrast Correction"
        '
        'TextBox_OCR_Gamma
        '
        Me.TextBox_OCR_Gamma.Location = New System.Drawing.Point(595, 93)
        Me.TextBox_OCR_Gamma.Name = "TextBox_OCR_Gamma"
        Me.TextBox_OCR_Gamma.Size = New System.Drawing.Size(47, 20)
        Me.TextBox_OCR_Gamma.TabIndex = 51
        Me.TextBox_OCR_Gamma.Text = "0.98"
        Me.TextBox_OCR_Gamma.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox_OCR_Contrast
        '
        Me.TextBox_OCR_Contrast.Location = New System.Drawing.Point(595, 67)
        Me.TextBox_OCR_Contrast.Name = "TextBox_OCR_Contrast"
        Me.TextBox_OCR_Contrast.Size = New System.Drawing.Size(47, 20)
        Me.TextBox_OCR_Contrast.TabIndex = 50
        Me.TextBox_OCR_Contrast.Text = "31"
        Me.TextBox_OCR_Contrast.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox_OCR_Confidence
        '
        Me.TextBox_OCR_Confidence.Location = New System.Drawing.Point(172, 142)
        Me.TextBox_OCR_Confidence.Name = "TextBox_OCR_Confidence"
        Me.TextBox_OCR_Confidence.Size = New System.Drawing.Size(47, 20)
        Me.TextBox_OCR_Confidence.TabIndex = 47
        Me.TextBox_OCR_Confidence.Text = "67"
        Me.TextBox_OCR_Confidence.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label_OCR_18
        '
        Me.Label_OCR_18.AutoSize = True
        Me.Label_OCR_18.Location = New System.Drawing.Point(17, 145)
        Me.Label_OCR_18.Name = "Label_OCR_18"
        Me.Label_OCR_18.Size = New System.Drawing.Size(111, 13)
        Me.Label_OCR_18.TabIndex = 46
        Me.Label_OCR_18.Text = "Confidence Threshold"
        '
        'NUD_OCR_Scaling
        '
        Me.NUD_OCR_Scaling.Location = New System.Drawing.Point(172, 168)
        Me.NUD_OCR_Scaling.Name = "NUD_OCR_Scaling"
        Me.NUD_OCR_Scaling.Size = New System.Drawing.Size(47, 20)
        Me.NUD_OCR_Scaling.TabIndex = 45
        Me.NUD_OCR_Scaling.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.NUD_OCR_Scaling.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        'Label_OCR_10
        '
        Me.Label_OCR_10.AutoSize = True
        Me.Label_OCR_10.Location = New System.Drawing.Point(17, 170)
        Me.Label_OCR_10.Name = "Label_OCR_10"
        Me.Label_OCR_10.Size = New System.Drawing.Size(107, 13)
        Me.Label_OCR_10.TabIndex = 44
        Me.Label_OCR_10.Text = "Image Scaling Factor"
        '
        'Button_OCR_ResetOCRSettings
        '
        Me.Button_OCR_ResetOCRSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_OCR_ResetOCRSettings.Location = New System.Drawing.Point(8, 209)
        Me.Button_OCR_ResetOCRSettings.Name = "Button_OCR_ResetOCRSettings"
        Me.Button_OCR_ResetOCRSettings.Size = New System.Drawing.Size(153, 23)
        Me.Button_OCR_ResetOCRSettings.TabIndex = 32
        Me.Button_OCR_ResetOCRSettings.Text = "Reset to default values"
        Me.Button_OCR_ResetOCRSettings.UseVisualStyleBackColor = True
        '
        'Button_OCR_TempFolder
        '
        Me.Button_OCR_TempFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_OCR_TempFolder.Location = New System.Drawing.Point(837, 38)
        Me.Button_OCR_TempFolder.Name = "Button_OCR_TempFolder"
        Me.Button_OCR_TempFolder.Size = New System.Drawing.Size(31, 20)
        Me.Button_OCR_TempFolder.TabIndex = 31
        Me.Button_OCR_TempFolder.Text = "..."
        Me.Button_OCR_TempFolder.UseVisualStyleBackColor = True
        '
        'TextBox_OCR_TempFolder
        '
        Me.TextBox_OCR_TempFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_OCR_TempFolder.Location = New System.Drawing.Point(172, 38)
        Me.TextBox_OCR_TempFolder.Name = "TextBox_OCR_TempFolder"
        Me.TextBox_OCR_TempFolder.Size = New System.Drawing.Size(659, 20)
        Me.TextBox_OCR_TempFolder.TabIndex = 30
        '
        'Label_OCR_8
        '
        Me.Label_OCR_8.AutoSize = True
        Me.Label_OCR_8.Location = New System.Drawing.Point(17, 42)
        Me.Label_OCR_8.Name = "Label_OCR_8"
        Me.Label_OCR_8.Size = New System.Drawing.Size(73, 13)
        Me.Label_OCR_8.TabIndex = 29
        Me.Label_OCR_8.Text = "Images Folder"
        '
        'Button_OCR_TessdataFolder
        '
        Me.Button_OCR_TessdataFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_OCR_TessdataFolder.Location = New System.Drawing.Point(837, 16)
        Me.Button_OCR_TessdataFolder.Name = "Button_OCR_TessdataFolder"
        Me.Button_OCR_TessdataFolder.Size = New System.Drawing.Size(31, 20)
        Me.Button_OCR_TessdataFolder.TabIndex = 28
        Me.Button_OCR_TessdataFolder.Text = "..."
        Me.Button_OCR_TessdataFolder.UseVisualStyleBackColor = True
        '
        'TextBox_OCR_Tessdata
        '
        Me.TextBox_OCR_Tessdata.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_OCR_Tessdata.Location = New System.Drawing.Point(172, 16)
        Me.TextBox_OCR_Tessdata.Name = "TextBox_OCR_Tessdata"
        Me.TextBox_OCR_Tessdata.Size = New System.Drawing.Size(659, 20)
        Me.TextBox_OCR_Tessdata.TabIndex = 27
        '
        'Label_OCR_7
        '
        Me.Label_OCR_7.AutoSize = True
        Me.Label_OCR_7.Location = New System.Drawing.Point(17, 17)
        Me.Label_OCR_7.Name = "Label_OCR_7"
        Me.Label_OCR_7.Size = New System.Drawing.Size(83, 13)
        Me.Label_OCR_7.TabIndex = 26
        Me.Label_OCR_7.Text = "Tessdata Folder"
        '
        'TabPage_Visibility
        '
        Me.TabPage_Visibility.Controls.Add(Me.TabControl_Visibility)
        Me.TabPage_Visibility.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Visibility.Name = "TabPage_Visibility"
        Me.TabPage_Visibility.Size = New System.Drawing.Size(876, 240)
        Me.TabPage_Visibility.TabIndex = 2
        Me.TabPage_Visibility.Text = "Visibility"
        Me.TabPage_Visibility.UseVisualStyleBackColor = True
        '
        'TabControl_Visibility
        '
        Me.TabControl_Visibility.Controls.Add(Me.TabPage_VIS_Systems)
        Me.TabControl_Visibility.Controls.Add(Me.TabPage_VIS_Stations)
        Me.TabControl_Visibility.Controls.Add(Me.TabPage_VIS_Commodities)
        Me.TabControl_Visibility.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl_Visibility.Location = New System.Drawing.Point(0, 0)
        Me.TabControl_Visibility.Name = "TabControl_Visibility"
        Me.TabControl_Visibility.SelectedIndex = 0
        Me.TabControl_Visibility.Size = New System.Drawing.Size(876, 240)
        Me.TabControl_Visibility.TabIndex = 0
        '
        'TabPage_VIS_Systems
        '
        Me.TabPage_VIS_Systems.Controls.Add(Me.ListView_VIS_Systems)
        Me.TabPage_VIS_Systems.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_VIS_Systems.Name = "TabPage_VIS_Systems"
        Me.TabPage_VIS_Systems.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_VIS_Systems.Size = New System.Drawing.Size(868, 214)
        Me.TabPage_VIS_Systems.TabIndex = 1
        Me.TabPage_VIS_Systems.Text = "Systems"
        Me.TabPage_VIS_Systems.UseVisualStyleBackColor = True
        '
        'ListView_VIS_Systems
        '
        Me.ListView_VIS_Systems.CheckBoxes = True
        Me.ListView_VIS_Systems.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView_VIS_Systems.Location = New System.Drawing.Point(3, 3)
        Me.ListView_VIS_Systems.Name = "ListView_VIS_Systems"
        Me.ListView_VIS_Systems.Size = New System.Drawing.Size(862, 208)
        Me.ListView_VIS_Systems.TabIndex = 0
        Me.ListView_VIS_Systems.UseCompatibleStateImageBehavior = False
        Me.ListView_VIS_Systems.View = System.Windows.Forms.View.List
        '
        'TabPage_VIS_Stations
        '
        Me.TabPage_VIS_Stations.Controls.Add(Me.ListView_VIS_Stations)
        Me.TabPage_VIS_Stations.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_VIS_Stations.Name = "TabPage_VIS_Stations"
        Me.TabPage_VIS_Stations.Size = New System.Drawing.Size(762, 214)
        Me.TabPage_VIS_Stations.TabIndex = 2
        Me.TabPage_VIS_Stations.Text = "Stations"
        Me.TabPage_VIS_Stations.UseVisualStyleBackColor = True
        '
        'ListView_VIS_Stations
        '
        Me.ListView_VIS_Stations.CheckBoxes = True
        Me.ListView_VIS_Stations.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView_VIS_Stations.Location = New System.Drawing.Point(0, 0)
        Me.ListView_VIS_Stations.Name = "ListView_VIS_Stations"
        Me.ListView_VIS_Stations.Size = New System.Drawing.Size(762, 214)
        Me.ListView_VIS_Stations.TabIndex = 0
        Me.ListView_VIS_Stations.UseCompatibleStateImageBehavior = False
        Me.ListView_VIS_Stations.View = System.Windows.Forms.View.List
        '
        'TabPage_VIS_Commodities
        '
        Me.TabPage_VIS_Commodities.Controls.Add(Me.ListView_VIS_Commodities)
        Me.TabPage_VIS_Commodities.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_VIS_Commodities.Name = "TabPage_VIS_Commodities"
        Me.TabPage_VIS_Commodities.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_VIS_Commodities.Size = New System.Drawing.Size(762, 214)
        Me.TabPage_VIS_Commodities.TabIndex = 0
        Me.TabPage_VIS_Commodities.Text = "Commodities"
        Me.TabPage_VIS_Commodities.UseVisualStyleBackColor = True
        '
        'ListView_VIS_Commodities
        '
        Me.ListView_VIS_Commodities.CheckBoxes = True
        Me.ListView_VIS_Commodities.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView_VIS_Commodities.Location = New System.Drawing.Point(3, 3)
        Me.ListView_VIS_Commodities.Name = "ListView_VIS_Commodities"
        Me.ListView_VIS_Commodities.Size = New System.Drawing.Size(756, 208)
        Me.ListView_VIS_Commodities.TabIndex = 0
        Me.ListView_VIS_Commodities.UseCompatibleStateImageBehavior = False
        Me.ListView_VIS_Commodities.View = System.Windows.Forms.View.List
        '
        'Button_SaveSettings
        '
        Me.Button_SaveSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_SaveSettings.Location = New System.Drawing.Point(796, 272)
        Me.Button_SaveSettings.Name = "Button_SaveSettings"
        Me.Button_SaveSettings.Size = New System.Drawing.Size(75, 23)
        Me.Button_SaveSettings.TabIndex = 1
        Me.Button_SaveSettings.Text = "Save"
        Me.Button_SaveSettings.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Options
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(883, 304)
        Me.Controls.Add(Me.Button_SaveSettings)
        Me.Controls.Add(Me.TabControl_Options)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(873, 245)
        Me.Name = "Options"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Options"
        Me.TabControl_Options.ResumeLayout(False)
        Me.TabPage_General.ResumeLayout(False)
        Me.TabPage_General.PerformLayout()
        Me.TabPage_OCR.ResumeLayout(False)
        Me.TabPage_OCR.PerformLayout()
        CType(Me.NUD_OCR_Scaling, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage_Visibility.ResumeLayout(False)
        Me.TabControl_Visibility.ResumeLayout(False)
        Me.TabPage_VIS_Systems.ResumeLayout(False)
        Me.TabPage_VIS_Stations.ResumeLayout(False)
        Me.TabPage_VIS_Commodities.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl_Options As System.Windows.Forms.TabControl
    Friend WithEvents TabPage_General As System.Windows.Forms.TabPage
    Friend WithEvents TabPage_OCR As System.Windows.Forms.TabPage
    Friend WithEvents Label_GEN_5 As System.Windows.Forms.Label
    Friend WithEvents TextBox_GEN_Filepath As System.Windows.Forms.TextBox
    Friend WithEvents Button_GEN_SelectDB As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Button_GEN_ResetGeneralSettings As System.Windows.Forms.Button
    Friend WithEvents Button_OCR_ResetOCRSettings As System.Windows.Forms.Button
    Friend WithEvents Button_OCR_TempFolder As System.Windows.Forms.Button
    Friend WithEvents TextBox_OCR_TempFolder As System.Windows.Forms.TextBox
    Friend WithEvents Label_OCR_8 As System.Windows.Forms.Label
    Friend WithEvents Button_OCR_TessdataFolder As System.Windows.Forms.Button
    Friend WithEvents TextBox_OCR_Tessdata As System.Windows.Forms.TextBox
    Friend WithEvents Label_OCR_7 As System.Windows.Forms.Label
    Friend WithEvents Button_SaveSettings As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label_GEN_19 As System.Windows.Forms.Label
    Friend WithEvents TextBox_GEN_ProcessName As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_OCR_Lang As System.Windows.Forms.TextBox
    Friend WithEvents Label_OCR_23 As System.Windows.Forms.Label
    Friend WithEvents Label_OCR_21 As System.Windows.Forms.Label
    Friend WithEvents Label_OCR_20 As System.Windows.Forms.Label
    Friend WithEvents TextBox_OCR_Gamma As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_OCR_Contrast As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_OCR_Confidence As System.Windows.Forms.TextBox
    Friend WithEvents Label_OCR_18 As System.Windows.Forms.Label
    Friend WithEvents NUD_OCR_Scaling As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label_OCR_10 As System.Windows.Forms.Label
    Friend WithEvents Label_OCR_6 As System.Windows.Forms.Label
    Friend WithEvents Label_OCR_4 As System.Windows.Forms.Label
    Friend WithEvents Label_OCR_2 As System.Windows.Forms.Label
    Friend WithEvents Label_OCR_1 As System.Windows.Forms.Label
    Friend WithEvents Label_GEN_9 As System.Windows.Forms.Label
    Friend WithEvents Label_OCR_14 As System.Windows.Forms.Label
    Friend WithEvents Label_OCR_13 As System.Windows.Forms.Label
    Friend WithEvents TextBox_OCR_BlackTH As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_OCR_Grayscale As System.Windows.Forms.TextBox
    Friend WithEvents Label_OCR_12 As System.Windows.Forms.Label
    Friend WithEvents Label_OCR_11 As System.Windows.Forms.Label
    Friend WithEvents Label_OCR_3 As System.Windows.Forms.Label
    Friend WithEvents Label_OCR_15 As System.Windows.Forms.Label
    Friend WithEvents CheckBox_OCR_DelScrsht As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage_Visibility As System.Windows.Forms.TabPage
    Friend WithEvents TabControl_Visibility As System.Windows.Forms.TabControl
    Friend WithEvents TabPage_VIS_Systems As System.Windows.Forms.TabPage
    Friend WithEvents ListView_VIS_Systems As System.Windows.Forms.ListView
    Friend WithEvents TabPage_VIS_Stations As System.Windows.Forms.TabPage
    Friend WithEvents TabPage_VIS_Commodities As System.Windows.Forms.TabPage
    Friend WithEvents ListView_VIS_Stations As System.Windows.Forms.ListView
    Friend WithEvents ListView_VIS_Commodities As System.Windows.Forms.ListView
    Friend WithEvents Label_OCR_5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox_OCR_PV As System.Windows.Forms.TextBox
End Class
