CREATE TABLE [dbo].[AbnormalAppeal] (
    [ID] [int] NOT NULL IDENTITY,
    [WorkAddress] [nvarchar](200),
    [AppealType] [nvarchar](100),
    [Reason] [nvarchar](200),
    [FinalTime] [datetime],
    [CreateTime] [datetime],
    [UserID] [int],
    [SignDataID] [int],
    [TimeDay] [datetime],
    [StartTime] [datetime],
    [EndTime] [datetime],
    [Status] [int],
    CONSTRAINT [PK_dbo.AbnormalAppeal] PRIMARY KEY ([ID])
)
CREATE TABLE [dbo].[AnnualLeave] (
    [ID] [int] NOT NULL IDENTITY,
    [UserID] [int],
    [LeaveID] [int],
    [BeginTime] [datetime],
    [FinishTime] [datetime],
    [Hours] [float],
    [Note] [nvarchar](500),
    [CreatTime] [datetime],
    [WorkYear] [nvarchar](50),
    [Status] [int],
    CONSTRAINT [PK_dbo.AnnualLeave] PRIMARY KEY ([ID])
)
CREATE TABLE [dbo].[GZ_Attendance] (
    [Id] [uniqueidentifier] NOT NULL,
    [DepartmentName] [nvarchar](50) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [Mobile] [nvarchar](20) NOT NULL,
    [Month] [nvarchar](7) NOT NULL,
    [TotalDays] [decimal](5, 2) NOT NULL,
    [RealDays] [decimal](5, 2) NOT NULL,
    [AbsenteeismDays] [decimal](5, 2) NOT NULL,
    [SickLeave] [decimal](5, 2) NOT NULL,
    [CompassionateLeave] [decimal](5, 2) NOT NULL,
    [BreakDown] [decimal](5, 2) NOT NULL,
    [AnnualLeave] [decimal](5, 2) NOT NULL,
    [OtherLeave] [decimal](5, 2) NOT NULL,
    [Remark] [nvarchar](1024),
    [Status] [int] NOT NULL,
    [FinalDays] [decimal](5, 2) NOT NULL,
    [DataSourceType] [int] NOT NULL,
    [CreateUser] [uniqueidentifier] NOT NULL,
    [CreateDate] [datetime] NOT NULL,
    [LastUpdateUser] [uniqueidentifier] NOT NULL,
    [LastUpdateDate] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.GZ_Attendance] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[Leave] (
    [ID] [int] NOT NULL IDENTITY,
    [UserID] [int],
    [CreateTime] [datetime],
    [StartTime] [datetime],
    [EndTime] [datetime],
    [Day] [nvarchar](50),
    [Hours] [nvarchar](50),
    [FinalTime] [datetime],
    [Reason] [nvarchar](200),
    [TypeName] [nvarchar](50),
    [Status] [int],
    [ExamineNote] [nvarchar](500),
    [AnnualLeaveDeal] [int],
    CONSTRAINT [PK_dbo.Leave] PRIMARY KEY ([ID])
)
CREATE TABLE [dbo].[OutsideApply] (
    [ID] [int] NOT NULL IDENTITY,
    [Reason] [nvarchar](200),
    [FinalTime] [datetime],
    [Day] [nvarchar](50),
    [Hours] [nvarchar](50),
    [StartTime] [datetime],
    [EndTime] [datetime],
    [CreateTime] [datetime],
    [UserID] [int],
    [TimeDay] [datetime],
    [Address] [nvarchar](200),
    [Status] [int],
    CONSTRAINT [PK_dbo.OutsideApply] PRIMARY KEY ([ID])
)
CREATE TABLE [dbo].[Schedul] (
    [ID] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](100),
    [CreateTime] [datetime],
    [IsAll] [int],
    [UserID] [nvarchar](50),
    [MstartTime] [datetime],
    [MendTime] [datetime],
    [AstartTime] [datetime],
    [AendTime] [datetime],
    [TimeDay] [date],
    [AdminID] [int],
    CONSTRAINT [PK_dbo.Schedul] PRIMARY KEY ([ID])
)
CREATE TABLE [dbo].[SchedulAttendLocation] (
    [ID] [int] NOT NULL IDENTITY,
    [SchedulID] [int],
    [AttendLocationID] [int],
    CONSTRAINT [PK_dbo.SchedulAttendLocation] PRIMARY KEY ([ID])
)
CREATE TABLE [dbo].[SignData] (
    [ID] [int] NOT NULL IDENTITY,
    [SchedulID] [nvarchar](50),
    [UserID] [int],
    [SignInTiem] [datetime],
    [SignOutTime] [datetime],
    [SinginType] [nvarchar](100),
    [SingoutType] [nvarchar](100),
    [SignInSchedulAttendLocationID] [nvarchar](50),
    [SignOutSchedulAttendLocationID] [nvarchar](50),
    [IsAbnormal] [int],
    [EffectiveTime] [nvarchar](50),
    [TimeDay] [datetime],
    [Leave] [nvarchar](50),
    [Belate] [nvarchar](50),
    [Outside] [nvarchar](50),
    [ForgotPunch] [nvarchar](50),
    [LeaveEarly] [nvarchar](50),
    [IsComplete] [int],
    CONSTRAINT [PK_dbo.SignData] PRIMARY KEY ([ID])
)
CREATE TABLE [dbo].[User] (
    [ID] [int] NOT NULL IDENTITY,
    [DepartmentID] [int],
    [TrueName] [nvarchar](50),
    [PhoneNum] [nvarchar](50),
    [headimage] [nvarchar](200),
    [JoinDate] [datetime],
    [IsExamine] [int],
    [BirthdayType] [nvarchar](50),
    [Birthday] [datetime],
    [CompleteDate] [datetime],
    [CreateDate] [datetime],
    [NoAbnormal] [int],
    [LeaveDate] [datetime],
    [Status] [int],
    [UserGuid] [nvarchar](500),
    CONSTRAINT [PK_dbo.User] PRIMARY KEY ([ID])
)
CREATE TABLE [dbo].[UserWages] (
    [ID] [int] NOT NULL IDENTITY,
    [UserID] [nvarchar](20),
    [UserName] [nvarchar](50),
    [Days] [nvarchar](50),
    [BaseWag] [nvarchar](50),
    [Other] [nvarchar](500),
    [TotalAmount] [nvarchar](50),
    [SocialSecurity] [nvarchar](50),
    [HousingFund] [nvarchar](50),
    [PersonalTax] [nvarchar](50),
    [Othercutpay] [nvarchar](50),
    [CutPayRemark] [nvarchar](500),
    [CutPayTitle] [nvarchar](50),
    [PayTrue] [nvarchar](50),
    [TimeMonth] [datetime],
    [JobSubsidies] [nvarchar](50),
    [JobLevel] [nvarchar](50),
    CONSTRAINT [PK_dbo.UserWages] PRIMARY KEY ([ID])
)
CREATE TABLE [dbo].[WagesCode] (
    [ID] [int] NOT NULL IDENTITY,
    [UserID] [nvarchar](50),
    [identifycode] [nvarchar](20),
    [Status] [int],
    [CodeCreateTime] [datetime],
    [TimeMonth] [nvarchar](50),
    CONSTRAINT [PK_dbo.WagesCode] PRIMARY KEY ([ID])
)
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](150) NOT NULL,
    [ContextKey] [nvarchar](300) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
)
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201810160635302_AutomaticMigration', N'Salary_MVC.Migrations.Configuration',  0x1F8B0800000000000400ED5DDB721BB9117D4F55FE81C567AF68C9716DE2A2768B262D471B495699B2B7B22F5BE00C244D792EDC198C227E5B1EF249F9850073C1E0361760008A72547EB11A83D3B876371A68F67FFFFD9FF9CF8F51387980691624F1E9F4F8E8F57402632FF183F8EE749AA3DB1FFE3AFDF9A73FFF69FEC18F1E275FEBEFDE90EF70CD383B9DDE23B47D379B65DE3D8C407614055E9A64C92D3AF2926806FC6476F2FAF5DF66C7C7338821A6186B32997FCE631444B0F803FFB94C620F6E510EC2CBC4876156D171C9BA409D5C8108665BE0C1D3E91A8420DDFD7EF97579B402084C278B3000B8196B18DE4E27208E1304106EE4BB2F195CA33489EFD65B4C00E1CD6E0BF177B720CC60D5F877CDE743FBF1FA84F463D654ACA1BC3C4349A40978FCA61A989958DD6878A774E0F0D07DC0438C76A4D7C5F09D4E179B384923102EB65B08C2E94464F96E19A6E473698C8B49392AF18E78945793E6DB57747DE06544FEBD9A2CF310E5293C8D618E52F2F575BE0903EF1F7077937C83F1699C8721DB68DC6C5CC61130E93A4DB63045BBCFF0B6EACAF96A3A99F1F56662455A8DA95376EF3C466F4EA6932BCC1C6C4248D70433146B94A4F0238C610A10F4AF014230C5537AEEC3621424EE02AF5F93F4DBC2F75398653553BC16F19E9A4E2EC1E3058CEFD0FDE914CFDC7472163C42BFA6540DF91207780BE24A28CD611FAF722208930E56C756587D8620234BD3718FCE82187708CB879A135E84B0FCBBBBE23285F44BBD9A5858A4D202E9AEB20EEE62B23B34AB91D6ACC04EB7816B045264D2B30FB16F520DF34379D6D1B3F9AC1130DD62278EB160BF80E0011ACB9C06E245E0D85ABFC5706AD6790FEF82D86439E12D1D64F72635FF9EE4295D87AB044F676F95AB0475C9C2B7568454216B4C3A4474C33F21483B5B68A181F676F0C7DF7E5FE0F518FB009B69A67B980379AA5DEC1BEC62BF1EC28F79E02B3671F72CACE0160BEE08EF62F2F7F839D764FF244C2F934D1076B13D71C43646F71D5C7F74C1F4066F8210EBF34644412F8888797D9DE2FF95C7A5B7D3C9DA030453A5057A6D2EA7F88B4D861727844116B964B30EBC6F950DE086C13289B6202338580E3BE5F41E4BFE6FABE45FB1B329612D26372C3EA17B983AE5F0191F57D36F9D479293BFB852739A6D2D0E1C2E573FD1866B6CC878903DA79935B63CE410836F94622A6188D1221B309A501720435FB6BE8D563550062D1B6CD08C3A8CBC1C43DA79191C43CCCFECFB3E1233A77657363B77D871C5C4D8BDB227F70F81B763B78E3C202956CE23888218EEE580C95801ABC25F3BFE20F7294759E0C3C5761BEE4CC51F8BF122059F729F18EFE3EF4590ED5B01ECD5BD6CE827DED7AD833DEF12B9E2F373E3FBA8AAFA8B2852F2EAD1A376AE85CCF7C579B608BB345BDF4E7225592E3353D17209CD64CBC298E3C290A3B180C11650A72CD3DDF9A55BF822F1AA8BEF517280077B910A4A5ED558696A247E682D2D81EA12D578D6ABFA2F133D6CA25D094CC3FBF3F3F8268091F6C11FD7C4C710239F01EE7D10EFE5B1046195E056EE8717194BA508743FF7D5743C11776C42542F92F44EF2B7B7D043C10367B6B86AA3A1AAE5FCF3AEDAF61E86A0C79B61814BE53570EEDA4AD2BB045DE7B1D7751168855531391F401A3A3F4B9F67E46E2B84CD2C8DD1B8A58BDE4CDB92BA2F9A56C9ABB9E7D73DE7E315B00F6FE7F57D12C3AB3C72CDE71E023F88C05D5787ECB8207E4982587D4BD4B79F2A37AEDED3A72045F73ED8F5E8733B32B9E2A5EDA0AAE484C9A874DD07F63DB732D2C0A567DB809F81DF9E08AEF23ED2AAD35E4BE8FE8AF7443646F216002FE2B775823B6DCD814F8EFAB9EC4356B34F119CC9189041BCA29C1B7EE4A189F3BBB2E215D6224AF218393FED245E00C235F4F2B45897CEEF51320C7C96C7DDB2CB8681806549426E94C0E35E168597A3ADFB8BA8658EAEC1AEF73192A537C105B39B0075BE7EB4335B984FF199FB932BF7AE72A892FE25D9ACF30D3EF305D0B928C3BC2EE0030CEDF219ACDC0BBDBC4CC8E9D64CB9538017E5AEE4B59F3B9FA068CDEDCE4B3AFD14560C09031396AC0FF38B36691FEF638F2C314380CF5969D5888FBFAD3684061F9162ABE059AE764B56B1E27B5462AE21A20F97F970C6A6256530E891F8C1AC078F7D752B83B1A53D4842BC84842594F7A0B5B568585BF8273F12085FDC83456FEA25185A320C41BCF16BC313BFEB43A7974932202DEAC128DD6352FD923CA06E75CA530254653D288C3A91509832D536A41B8E96CD676504754598CF5A42ADE79760BBC5028109BDAE2893751977BDFC61AD1F931C9518332F538426D3D6524E584DE00E0AA5A5E83B0BD20C9119DC002293967E247DC68A979621AE39292588A8289BD1AFABD52F1325752ECA1A01AA19CD33DC41E2242CFA0A69BBFADA524090D7E72055E8E16512E651DCA6CBBB6A7371CA2C0C57301C8F8D4566E158FA70B4FA1D1D8B54D386A330CFE45820863C1C8B55C02C184B1F8E56DB352C524D1B8EC2C621B3482C7D381ABDAA62A12851A355CD7316AE510D7938167DAEC72251A2569B0AAB4B6850419351E63361CF8AF261260908C18616C5CE30A1C4181AA324523BCE1071D455DB8D2CB2B31B6854330B4389C3719848671689216B49201AFC2C88204A1F8E563DAD65812AD2708CF2013B0B51523425618B20D4ED5113112DAAA092FA9DED70DEFC1FB3C73B9106ECF29EFAADFBDC17F6B91456DD555B8C886691C4328D152D61E922D451CB2C464DD341290ED93C4841D2D0C14D4C31A7851BB2960DA5806AA81AB69D1828CC197862A18EFD42238379F3859235A492220898134F8A720D9DD004FE723AA1216B8C26EB4FE046B2DDD1D085C706F4B2702C5D67D594FE727ECD943457D2B8D37E97571C43D6903D421C2E277B8432DD5341E922904F052AD7413F5A79312EA395740D7B4888CDE5CC22A1CC04556EA75876307A77BC4D6D6A4D3F433BDAEE49F7104F83D22957F3846BC30EB7E99BB0E32DA917BF607A50EABEA53F1771CBCD385B60A477CBA8DA16DDBB525C1914A84F23BB383FFD1811D60534409275577723D00ECF0D7808A2E31045EA21BA476D3934956E720317F9B37051D4F77963244D0BC60021D35AD38D7C19EF3AB0BBEEABA058AE432569DF7B870D83E53C1A998994694263392CA82F67162DED5A18B56BA16CD7C2A05DF6644D1563CBCB9A8A78685242B8A5B72033BA11874B903E1C37F28409BAE4847C43D6580952341DB724A4D2C3591BF5838B51CBA10564C80A68AD7AF8936EEF5EB88EAF15EF856BBA1E1A8DB915E168810E5E1388CBC335743D341A6B2BC2D102DDB16B8D689587B3F553ED111ECCB4EB5B2D638306EF081607A56B1C24F8A85AEE38C117ED5FA92A9CE3FA370055B02C7F251C6AFA6469302CE7A8AF891A475A36DA953BD4B2059A235485B44AC354D175565613B6CAAFAC867E30EAAA70808F51552A80016A4A5DCD8D8AE24354D5D7AE9A676B1ABBCAED4D4A1D8ED444A7B2480D753812137FCA4231E4E1584D84290BD5507536038D39E5F702256B08212E129513455C893EA21A4DF702B6894015AF5E9B92A7BA0E636355F9E72F26EA8E096195E4A56ECBECB8CB9B4057D1782CA9072570CBF7D063A5AE1265A0E86DA97BC817734D0CAA88A32B73E53B75DDEB741A43CA898D9AA8F97841F16E41FBD14C1D022A3D9BA90B34F6A310E7C9ED4BA14CCBF3DFC4730AFEFFA640436FB2419B9CEA640B3467A28ECC94E6A32ED090DE5CF82527BFB9125DC42AC65206AC0A3446B00EA4E446AF26EA1D53148FBF18B28EDDC1064FF2B6075BA2855885480A6815F560F44213E132462FB4A20CD00B1D750F592FF0E18B2C165FB26F8B448C5BE46D42BECCED6E73BAA2A5102CF113CABDA2D0BF69085615FED49F02538A872A3F21D92A92073CDB299EA75D8660547951FF0897610089FAAB3FB80471700B3354C60D4F4F5E1F9F0889340F27A9E52CCB7CEE2EB037B3253F6B7B08810EC8D8F606396BA6C250E4908C1F40EADD9327F1E28F2935D866292395C8C7FAC8FC4F9FDB6AAFF44BE7E4752112128298FD52B129121F935E2C003D00397DA40188F0C382A69D917E48DD1448F861F511ED61C2E37BC7C52815E5F72124462F43210BA401829413D274D6E51C91A6485CF681DB3001DA9D62337C28A5D85B7D2926658C34ED9E9841B2A58187B2EB3AA28CF691B6318F833F725819C101712A5849E13860D0B53334DAC2E41330B668E171F91595A03F8E4F9FE83B4E9F681BBF257DA26D3652FA44DB0CDAD327DAE624A54FB43E2572FA44DB2CE4F489F6D72DFB8B652DE63949A06851C69BE54AB4DD7375AE4493A6CA991247EB02F95752554A7C44D6C4D12D5467501CD84ACD0C8A2F36B4E553E5A11DC498B3A525239333CD2D615A730FB8F15E34C1534F69AE2BD6889CCCD0D651A72577A1BD33457B1CD2B3154307EE3A7B1EA2E0D004E801391C2DF90A1DB9A45D391F94B144CF5646F4AA110397BDBD25CA65F33358A1FC12B72411E4547EA6DD1353FB19EF206B2D1253FF99E2284483BE58E0D2025ADFC05DA13CCF763B4BE9D90C364D5BBE2D8B33A00CA5F97E06DD92A0B17223C827C4333E46CA09F2CCA1C48479B6F48E223F9E3DE801E9F06C599C83B2DF59622627BB3339F7A952DF596AA025239373AD5A6A1A9FE8CE12A890D7CE9643434E63670959CE5A676D658A49EAEC29203948E6D92A1F559A3893B39C9034CED2248A39E22CC14A29E16C1D1AC50C70E6071721239CC93308457E385B924B480767EC8C50A4871BE7D8B08124A78F337DC662A335A3BDAA62AA393397AA4166B9EF43400EF0019C1899E60EC4257B0B6A6BABF359D96C59296C12365B3E7E45CE355B26B532C59A3DAFB39851CD96F69413A8D99C3F3E5F9A2564557A346BEFDDE46C68B6469A4F7E66F1ECC2BD7132D521AADC6796DA28A63A338235C86CF67FA35CF4A7449537CC92DE1A6D8CA893868D7113F73F021CB7FAC49C6172EE1FBE89342487BEC7D3CC0A56C6CEE041D924B85B65A3DD650E537273975A4CC5CE6AF63115030789C9546C6CA72E53F1D8535EB30ED62E52A029D9594990A642B6993AAD0DDF6A66351593F189D7E4E8BEF9EC33B69DB14C2DFF5AC10CCF01859863CC98F86993B801ADBF398F6F935A38E3FEB12DAA3F912E2F11C0721F2C52ACAB808770B107B3ACC801F9158439FEE443B481FE3971666F73B4C832186DF89D359F75F32FB2CBF16D9E7FDA92BF321B5DC0CD0C88EAFA14BFCF83D0A7ED3E53BC636C81203358D913B8556B44EC8ABB1D45BA4AC45F626903AA866F05B7787762FD7F03A32D712F679FE27521FDF4DB86D7F105BC03DEEEBA0AD26C07E99F087ED8E7AB00DCA520CA2A8CA63EFE13AF613F7AFCE97FB6DB11A40DAE0000 , N'6.1.3-40302')

