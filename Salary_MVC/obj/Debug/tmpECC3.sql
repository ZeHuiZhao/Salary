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
    [LastUpdateUser] [uniqueidentifier] NOT NULL,
    [LastUpdateDate] [datetime] NOT NULL,
    [CreateUser] [uniqueidentifier] NOT NULL,
    [CreateDate] [datetime] NOT NULL,
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
VALUES (N'201810160916509_AutomaticMigration', N'Salary_MVC.Migrations.Configuration',  0x1F8B0800000000000400ED5DDB721BB9117D4F55FE81C567AF68C9716DE2A2768B262D471B495699B2B7B22F5BE00C244D792EDC198C227E5B1EF249F9850073C1E0361760008A72547EB11A83D3B876371A68F67FFFFD9FF9CF8F51387980691624F1E9F4F8E8F57402632FF183F8EE749AA3DB1FFE3AFDF9A73FFF69FEC18F1E275FEBEFDE90EF70CD383B9DDE23B47D379B65DE3D8C407614055E9A64C92D3AF2926806FC6476F2FAF5DF66C7C7338821A6186B32997FCE631444B0F803FFB94C620F6E510EC2CBC4876156D171C9BA409D5C8108665BE0C1D3E91A8420DDFD7EF97579B402084C278B3000B8196B18DE4E27208E1304106EE4BB2F195CA33489EFD65B4C00E1CD6E0BF177B720CC60D5F877CDE743FBF1FA84F463D654ACA1BC3C4349A40978FCA61A989958DD6878A774E0F0D07DC0438C76A4D7C5F09D4E179B384923102EB65B08C2E94464F96E19A6E473698C8B49392AF18E78945793E6DB57747DE06544FEBD9A2CF310E5293C8D618E52F2F575BE0903EF1F7077937C83F1699C8721DB68DC6C5CC61130E93A4DB63045BBCFF0B6EACAF96A3A99F1F56662455A8DA95376EF3C466F4EA6932BCC1C6C4248D70433146B94A4F0238C610A10F4AF014230C5537AEEC3621424EE02AF5F93F4DBC2F75398653553BC16F19E9A4E2EC1E3058CEFD0FDE914CFDC7472163C42BFA6540DF91207780BE24A28CD611FAF722208930E56C756587D8620234BD3718FCE82187708CB879A135E84B0FCBBBBE23285F44BBD9A5858A4D202E9AEB20EEE62B23B34AB91D6ACC04EB7816B045264D2B30FB16F520DF34379D6D1B3F9AC1130DD62278EB160BF80E0011ACB9C06E245E0D85ABFC5706AD6790FEF82D86439E12D1D64F72635FF9EE4295D87AB044F676F95AB0475C9C2B7568454216B4C3A4474C33F21483B5B68A181F676F0C7DF7E5FE0F518FB009B69A67B980379AA5DEC1BEC62BF1EC28F79E02B3671F72CACE0160BEE08EF62F2F7F839D764FF244C2F934D1076B13D71C43646F71D5C7F74C1F4066F8210EBF34644412F8888797D9DE2FF95C7A5B7D3C9DA030453A5057A6D2EA7F88B4D861727844116B964B30EBC6F950DE086C13289B6202338580E3BE5F41E4BFE6FABE45FB1B329612D26372C3EA17B983AE5F0191F57D36F9D479293BFB852739A6D2D0E1C2E573FD1866B6CC878903DA79935F60264E8CBD6C7AB9C187DA3945303458C17D990D1DD82C5F16B74AB4A1883160D3668461D465E8E21EDBC0C8E21E667F67D1F899953BB2B9B9D3BECB86262EC5ED993FB87C0DBB15B471E90142BE71144410CF772C064AC8055E1AF1D7F90FB94A32CF0E162BB0D77A6E28FC57891824FB94F8CF7F1F722C8F6AD00F6EA5E36F413EFEBD6C19E77895CF1F9B9F17D5455FD45142979F5E8513BD742E6FBE23C5B845D9AAD6F27B9922C9799A968B98466B26561CC7161C8D158C0600BA85396E9EEFCD22D7C9178D5C5F72839C083BD480525AF6AAC3435123FB496964075896A3CEB55FD97891E36D1AE04A6E1FDF9797C13C048FBE08F6BE2638891CF00F73E88F7F25882B04A702BF7C38B8CA55204BA9FFB6A3A9E883B3621AA17497A27F9DB5BE8A1E081335B5CB5D150D572FE79576D7B0F43D0E3CDB0C0A5F21A38776D25E95D82AEF3D8EBBA08B4C2AA989C0F200D9D9FA5CF3372B715C26696C668DCD23D6FA66D49DD174DABE4D5DCF3EB9EF3F10AD887B7F3FA3E89E1551EB9E6730F811F44E0AEAB43765C10BF2441ACBE25EADB4F951B57EFE95390A27B1FEC7AF4B91D995CF1D276505572C26454BA6EDDFA9E5B1969E0D2B36DC0CFC06F4F0457791769D569AF25747FC57B221B23790B8017F1DB3AC19DB6E6C02747FD5CF621ABD9A708CE640CC8205E51CE0D3FF2D0C4F95D59F10A6B1125798C9C9F76122F00E11A7A795AAC4BE7F72819063ECBE36ED965C340C0B22421374AE0712F8BC2CBD1D6FD45D43247D760D7FB18C9D29BE082D94D803A5F3FDA992DCCA7F8CCFDC9957B57395449FF926CD6F9069FF902E85C94615E17F0018676F90C56EE855E5E26E4746BA6DC29C08B7257F2DACF9D4F50B4E676E7259D7E0A2B868481094BD687F9459BB48FF7B147969821C0E7ACB46AC4C7DF561B42838F48B155F02C57BB25AB58F13D2A31D710D187CB7C3863D3923218F448FC60D683C7BEBA95C1D8D21E24215E42C212CA7BD0DA5A34AC2DFC931F09842FEEC1A237F5120C2D198620DEF8B5E189DFF5A1D3CB24199016F56094EE31A97E491E50B73AE52901AAB21E14469D48284C996A1BD20D47CBE6B33282BA22CC672DA1D6F34BB0DD6281C0845E5794C9BA8CBB5EFEB0D68F498E4A8C9997294293696B2927AC26700785D252F49D056986C80C6E0091494B3F923E63C54BCB10D79C9412445494CDE8D7D5EA9789923A17658D00D58CE619EE207112167D85B45D7D6D2920C8EB73902AF4F03209F3286ED3E55DB5B9386516862B188EC7C622B3702C7D385AFD8E8E45AA69C3519867722C10431E8EC52A60168CA50F47ABED1A16A9A60D4761E3905924963E1C8D5E55B15094A8D1AAE6390BD7A8863C1C8B3ED763912851AB4D85D52534A8A0C928F399B06745F93093048460438B62679850620C8D5112A91D678838EAAAED4616D9D90D34AA9985A1C4E1384CA4338BC490B524100D7E164410A50F47AB9ED6B24015693846F9809D8528299A92B04510EAF6A8898816555049FDCE76386FFE8FD9E39D480376794FFDD67DEE0BFB5C0AABEEAA2D4644B3486299C68A96B07411EAA86516A3A6E9A014876C1EA42069E8E026A698D3C20D59CB86524035540DDB4E0C14E60C3CB150C77EA191C1BCF942C91A52491104CC892745B9864E68027F399DD090354693F5277023D9EE68E8C263037A593896AEB36A4A7F39BF664A9A2B69DC69BFCB2B8E216BC81E210E97933D429986CD2144E472A6875066825A5E92AB51CB32DD138CDC4E96AE8B26B78FA51F8CDE1D6F539B5AD3CFD08EB67BD23DC4D3A074CAD53CE1DAB0C36DFA26EC784BEAC52F981E94BA6FE9CF45DC7233CE1618E9DD32AAB645F7AE14570605EAD3C82ECE4F3F468475010D9064DDD5DD08B4C373031E82E83844917A88EE515B0E4DA59BDCC045FE2C5C14F57DDE1849D3823140C8B4D674235FC6BB0EECAEFB2A2896EB5049DAF7DE61C360398F466622659AD0580E0BEACB99454BBB1646ED5A28DBB53068973D5953C5D8F2B2A6221E9A94106EE92DC88C6EC4E112A40FC78D3C61822E3921DF90355682144DC72D09A9F470D646FDE062D472680119B2025AAB1EFEA4DBBB17AEE36BC57BE19AAE8746636E45385AA083D704E2F2700D5D0F8DC6DA8A70B44077EC5A235AE5E16CFD547B840733EDFA56CBD8A0C13B82C541E91A07093EAA963B4EF045FB57AA0AE7B8FE0D40152CCB5F09879A3E581A0CCB39EA6BA2C691968D76E50EB56C81E6085521ADD23055749D95D584ADF22BABA11F8CBA2A1CDF6354950A60809A525773A3A2F81055F5B5ABE6D99AC6AE727B9352872335D1A92C52431D8EC4C49FB2500C7938561361CA4235549DCD40634EF9BD40C91A42888B44E5441157A28FA846D3BD806D2250C5ABD7A6C4F51553EB593F51AB3B96AE293015B7730D79DFEEF226D055341E4BEA4109DCF23DF458A9AB4419287A5BEA1EF2C55C13832AE2E8CA5CF94E5DF73A9DC6907262A3266A3E5E50BC5BD07E345387804ACF66EA028DFD28C47972FB5228D3F2FC37F19C82FFBF29D0D09B6CD026A73AD902CD99A82333A5F9A80B34A437177EC9C96FAE4417B18AB19401AB028D11AC0329B9D1AB897AC714C5E32F86AC6377B0C193BCEDC1966821562192025A453D18BDD044B88CD10BAD2803F44247DD43D60B7CF8228BC597ECDB2211E316799B902F73BBDB9CAE6829044BFC8472AF28F46F1A8255853FF5A7C094E2A1CA4F48B68AE401CF768AE769972118555ED43FC2651840A2FEEA0F2E411CDCC20C9571C3D393D7C7274222CDC3496A39CB329FBB0BECCD6CC9CFDA1E42A00332B6BD41CE9A6930143924E307907AF7E449BCF8634A0DB659CA4825F2B13E32FFD3E7B6DA2BFDD23979F98884842066BF546C8AC4C7A4170B400F404E1F690022FCB0A06967A41F523705127E587D447B98F0F8DE71314A45F97D0889D1CB50C802698020E584349D753947A42912977DE0364C8076A7D80C1F4A29F6565F8A4919234DBB2766906C69E0A1ECBA8E28A37DA46DCCE3E08F1C564670409C0A5652380E1874ED0C8DB630F9048C2D5A785C7E4525E88FE3D327FA8ED327DAC66F499F689B8D943ED13683F6F489B63949E913AD4F899C3ED1360B397DA2FD75CBFE62598B794E12285A94F166B9126DF75C9D2BD1A4A9EA4C89A3F5813A6BA24A991B664D1CDD42F9B75C07B64E3383E28B0D6DF95479680731E66C69C9C8E44C734B98D6DC036EBC174DF0D4539AEB8A35222733B475D469C95D68EF4CD11E87F46CC5D081BBCE9E87283834017A400E474BBE42472E6957CE07652CD1B39511BD6AC4C0656F6F8972D9FC0C5628BFC42D490439959F69F7C4D47EC63BC85A8BC4D47FA6380AD1A02F16B8B480D637705728CFB3DDCE527A36834DD3966FCBE20C284369BE9F41B72468ACDC08F209F18C8F9172823C732831619E2DBDA3C88F670F7A403A3C5B16E7A0EC779698C9C9EE4CCE7DAAD477961A68C9C8E45CAB969AC627BAB3042AE4B5B3E5D090D3D8594296B3D6595B9962923A7B0A480E9279B6CA479526CEE42C27248DB33489628E384BB0524A385B874631039CF9C145C80867F20C42911FCE96E412D2C1193B2314E9E1C639366C20C9E9E34C9FB1D868CD68AFAA986ACECCA56A9059EEFB1090037C002746A6B90371C9DE82DADAEA7C56365B560A9B84CD968F5F9173CD9649AD4CB166CFEB2C6654B3A53DE5046A36E78FCF97660959951ECDDA7B37391B9AAD91E6939F593CBB706F9C4C75882AF799A5368AA9CE8C600D329BFDDF2817FD2951E50DB3A4B7461B23EAA46163DCC4FD8F00C7AD3E3167989CFB876F220DC9A1EFF134B38295B13378503609EE56D9687799C394DCDCA51653B1B39A7D4CC5C0416232151BDBA9CB543CF694D7AC83B58B14684A765612A4A9906DA64E6BC3B79A594DC5647CE23539BA6F3EFB8C6D672C53CBBF5630C3734021E61833267EDA246E40EB6FCEE3DBA416CEB87F6C8BEA4FA4CB4B04B0DC078B14EB2AE0215CECC12C2B72407E05618E3FF9106DA07F4E9CD9DB1C2DB20C461B7E67CD67DDFC8BEC727C9BE79FB6E4AFCC4617703303A2BA3EC5EFF320F469BBCF14EF185B20C80C56F6046ED51A11BBE26E4791AE12F19758DA80AAE15BC12DDE9D58FFDFC0684BDCCBD9A7785D483FFDB6E1757C01EF80B7BBAE8234DB41FA27821FF6F92A00772988B20AA3A98FFFC46BD88F1E7FFA1F4D01A8A20DAE0000 , N'6.1.3-40302')
