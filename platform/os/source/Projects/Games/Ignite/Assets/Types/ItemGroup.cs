// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\ItemDropTable.txt\\ItemGroup")]
public class ItemGroup
{
    [JsonPropertyName("0")]
    public string MapArea { get; set; }

    [JsonPropertyName("1")]
    public string MobId { get; set; }

    [JsonPropertyName("2")]
    public short MinLevel { get; set; }

    [JsonPropertyName("3")]
    public short MaxLevel { get; set; }

    [JsonPropertyName("4")]
    public byte AbStateCnt { get; set; }

    [JsonPropertyName("5")]
    public int MinCen { get; set; }

    [JsonPropertyName("6")]
    public int MaxCen { get; set; }

    [JsonPropertyName("7")]
    public int CenRate { get; set; }

    [JsonPropertyName("8")]
    public string TradeBoxA { get; set; }

    [JsonPropertyName("9")]
    public int RateA { get; set; }

    [JsonPropertyName("10")]
    public string TradeBoxB { get; set; }

    [JsonPropertyName("11")]
    public int RateB { get; set; }

    [JsonPropertyName("12")]
    public string TradeBoxC { get; set; }

    [JsonPropertyName("13")]
    public int RateC { get; set; }

    [JsonPropertyName("14")]
    public string DrItem1 { get; set; }

    [JsonPropertyName("15")]
    public int DrItem1R { get; set; }

    [JsonPropertyName("16")]
    public byte UpGradeMin01 { get; set; }

    [JsonPropertyName("17")]
    public byte UpGradeMax01 { get; set; }

    [JsonPropertyName("18")]
    public string Rule1 { get; set; }

    [JsonPropertyName("19")]
    public short Num1 { get; set; }

    [JsonPropertyName("20")]
    public string DrItem2 { get; set; }

    [JsonPropertyName("21")]
    public int DrItem2R { get; set; }

    [JsonPropertyName("22")]
    public byte UpGradeMin02 { get; set; }

    [JsonPropertyName("23")]
    public byte UpGradeMax02 { get; set; }

    [JsonPropertyName("24")]
    public string Rule2 { get; set; }

    [JsonPropertyName("25")]
    public short Num2 { get; set; }

    [JsonPropertyName("26")]
    public string DrItem3 { get; set; }

    [JsonPropertyName("27")]
    public int DrItem3R { get; set; }

    [JsonPropertyName("28")]
    public byte UpGradeMin03 { get; set; }

    [JsonPropertyName("29")]
    public byte UpGradeMax03 { get; set; }

    [JsonPropertyName("30")]
    public string Rule3 { get; set; }

    [JsonPropertyName("31")]
    public short Num3 { get; set; }

    [JsonPropertyName("32")]
    public string DrItem4 { get; set; }

    [JsonPropertyName("33")]
    public int DrItem4R { get; set; }

    [JsonPropertyName("34")]
    public byte UpGradeMin04 { get; set; }

    [JsonPropertyName("35")]
    public byte UpGradeMax04 { get; set; }

    [JsonPropertyName("36")]
    public string Rule4 { get; set; }

    [JsonPropertyName("37")]
    public short Num4 { get; set; }

    [JsonPropertyName("38")]
    public string DrItem5 { get; set; }

    [JsonPropertyName("39")]
    public int DrItem5R { get; set; }

    [JsonPropertyName("40")]
    public byte UpGradeMin05 { get; set; }

    [JsonPropertyName("41")]
    public byte UpGradeMax05 { get; set; }

    [JsonPropertyName("42")]
    public string Rule5 { get; set; }

    [JsonPropertyName("43")]
    public short Num5 { get; set; }

    [JsonPropertyName("44")]
    public string DrItem6 { get; set; }

    [JsonPropertyName("45")]
    public int DrItem6R { get; set; }

    [JsonPropertyName("46")]
    public byte UpGradeMin06 { get; set; }

    [JsonPropertyName("47")]
    public byte UpGradeMax06 { get; set; }

    [JsonPropertyName("48")]
    public string Rule6 { get; set; }

    [JsonPropertyName("49")]
    public short Num6 { get; set; }

    [JsonPropertyName("50")]
    public string DrItem7 { get; set; }

    [JsonPropertyName("51")]
    public int DrItem7R { get; set; }

    [JsonPropertyName("52")]
    public byte UpGradeMin07 { get; set; }

    [JsonPropertyName("53")]
    public byte UpGradeMax07 { get; set; }

    [JsonPropertyName("54")]
    public string Rule7 { get; set; }

    [JsonPropertyName("55")]
    public short Num7 { get; set; }

    [JsonPropertyName("56")]
    public string DrItem8 { get; set; }

    [JsonPropertyName("57")]
    public int DrItem8R { get; set; }

    [JsonPropertyName("58")]
    public byte UpGradeMin08 { get; set; }

    [JsonPropertyName("59")]
    public byte UpGradeMax08 { get; set; }

    [JsonPropertyName("60")]
    public string Rule8 { get; set; }

    [JsonPropertyName("61")]
    public short Num8 { get; set; }

    [JsonPropertyName("62")]
    public string DrItem9 { get; set; }

    [JsonPropertyName("63")]
    public int DrItem9R { get; set; }

    [JsonPropertyName("64")]
    public byte UpGradeMin09 { get; set; }

    [JsonPropertyName("65")]
    public byte UpGradeMax09 { get; set; }

    [JsonPropertyName("66")]
    public string Rule9 { get; set; }

    [JsonPropertyName("67")]
    public short Num9 { get; set; }

    [JsonPropertyName("68")]
    public string DrItem10 { get; set; }

    [JsonPropertyName("69")]
    public int DrItem10R { get; set; }

    [JsonPropertyName("70")]
    public byte UpGradeMin10 { get; set; }

    [JsonPropertyName("71")]
    public byte UpGradeMax10 { get; set; }

    [JsonPropertyName("72")]
    public string Rule10 { get; set; }

    [JsonPropertyName("73")]
    public short Num10 { get; set; }

    [JsonPropertyName("74")]
    public string DrItem11 { get; set; }

    [JsonPropertyName("75")]
    public int DrItem11R { get; set; }

    [JsonPropertyName("76")]
    public byte UpGradeMin11 { get; set; }

    [JsonPropertyName("77")]
    public byte UpGradeMax11 { get; set; }

    [JsonPropertyName("78")]
    public string Rule11 { get; set; }

    [JsonPropertyName("79")]
    public short Num11 { get; set; }

    [JsonPropertyName("80")]
    public string DrItem12 { get; set; }

    [JsonPropertyName("81")]
    public int DrItem12R { get; set; }

    [JsonPropertyName("82")]
    public byte UpGradeMin12 { get; set; }

    [JsonPropertyName("83")]
    public byte UpGradeMax12 { get; set; }

    [JsonPropertyName("84")]
    public string Rule12 { get; set; }

    [JsonPropertyName("85")]
    public short Num12 { get; set; }

    [JsonPropertyName("86")]
    public string DrItem13 { get; set; }

    [JsonPropertyName("87")]
    public int DrItem13R { get; set; }

    [JsonPropertyName("88")]
    public byte UpGradeMin13 { get; set; }

    [JsonPropertyName("89")]
    public byte UpGradeMax13 { get; set; }

    [JsonPropertyName("90")]
    public string Rule13 { get; set; }

    [JsonPropertyName("91")]
    public short Num13 { get; set; }

    [JsonPropertyName("92")]
    public string DrItem14 { get; set; }

    [JsonPropertyName("93")]
    public int DrItem14R { get; set; }

    [JsonPropertyName("94")]
    public byte UpGradeMin14 { get; set; }

    [JsonPropertyName("95")]
    public byte UpGradeMax14 { get; set; }

    [JsonPropertyName("96")]
    public string Rule14 { get; set; }

    [JsonPropertyName("97")]
    public short Num14 { get; set; }

    [JsonPropertyName("98")]
    public string DrItem15 { get; set; }

    [JsonPropertyName("99")]
    public int DrItem15R { get; set; }

    [JsonPropertyName("100")]
    public byte UpGradeMin15 { get; set; }

    [JsonPropertyName("101")]
    public byte UpGradeMax15 { get; set; }

    [JsonPropertyName("102")]
    public string Rule15 { get; set; }

    [JsonPropertyName("103")]
    public short Num15 { get; set; }

    [JsonPropertyName("104")]
    public string DrItem16 { get; set; }

    [JsonPropertyName("105")]
    public int DrItem16R { get; set; }

    [JsonPropertyName("106")]
    public byte UpGradeMin16 { get; set; }

    [JsonPropertyName("107")]
    public byte UpGradeMax16 { get; set; }

    [JsonPropertyName("108")]
    public string Rule16 { get; set; }

    [JsonPropertyName("109")]
    public short Num16 { get; set; }

    [JsonPropertyName("110")]
    public string DrItem17 { get; set; }

    [JsonPropertyName("111")]
    public int DrItem17R { get; set; }

    [JsonPropertyName("112")]
    public byte UpGradeMin17 { get; set; }

    [JsonPropertyName("113")]
    public byte UpGradeMax17 { get; set; }

    [JsonPropertyName("114")]
    public string Rule17 { get; set; }

    [JsonPropertyName("115")]
    public short Num17 { get; set; }

    [JsonPropertyName("116")]
    public string DrItem18 { get; set; }

    [JsonPropertyName("117")]
    public int DrItem18R { get; set; }

    [JsonPropertyName("118")]
    public byte UpGradeMin18 { get; set; }

    [JsonPropertyName("119")]
    public byte UpGradeMax18 { get; set; }

    [JsonPropertyName("120")]
    public string Rule18 { get; set; }

    [JsonPropertyName("121")]
    public short Num18 { get; set; }

    [JsonPropertyName("122")]
    public string DrItem19 { get; set; }

    [JsonPropertyName("123")]
    public int DrItem19R { get; set; }

    [JsonPropertyName("124")]
    public byte UpGradeMin19 { get; set; }

    [JsonPropertyName("125")]
    public byte UpGradeMax19 { get; set; }

    [JsonPropertyName("126")]
    public string Rule19 { get; set; }

    [JsonPropertyName("127")]
    public short Num19 { get; set; }

    [JsonPropertyName("128")]
    public string DrItem20 { get; set; }

    [JsonPropertyName("129")]
    public int DrItem20R { get; set; }

    [JsonPropertyName("130")]
    public byte UpGradeMin20 { get; set; }

    [JsonPropertyName("131")]
    public byte UpGradeMax20 { get; set; }

    [JsonPropertyName("132")]
    public string Rule20 { get; set; }

    [JsonPropertyName("133")]
    public short Num20 { get; set; }

    [JsonPropertyName("134")]
    public string DrItem21 { get; set; }

    [JsonPropertyName("135")]
    public int DrItem21R { get; set; }

    [JsonPropertyName("136")]
    public byte UpGradeMin21 { get; set; }

    [JsonPropertyName("137")]
    public byte UpGradeMax21 { get; set; }

    [JsonPropertyName("138")]
    public string Rule21 { get; set; }

    [JsonPropertyName("139")]
    public short Num21 { get; set; }

    [JsonPropertyName("140")]
    public string DrItem22 { get; set; }

    [JsonPropertyName("141")]
    public int DrItem22R { get; set; }

    [JsonPropertyName("142")]
    public byte UpGradeMin22 { get; set; }

    [JsonPropertyName("143")]
    public byte UpGradeMax22 { get; set; }

    [JsonPropertyName("144")]
    public string Rule22 { get; set; }

    [JsonPropertyName("145")]
    public short Num22 { get; set; }

    [JsonPropertyName("146")]
    public string DrItem23 { get; set; }

    [JsonPropertyName("147")]
    public int DrItem23R { get; set; }

    [JsonPropertyName("148")]
    public byte UpGradeMin23 { get; set; }

    [JsonPropertyName("149")]
    public byte UpGradeMax23 { get; set; }

    [JsonPropertyName("150")]
    public string Rule23 { get; set; }

    [JsonPropertyName("151")]
    public short Num23 { get; set; }

    [JsonPropertyName("152")]
    public string DrItem24 { get; set; }

    [JsonPropertyName("153")]
    public int DrItem24R { get; set; }

    [JsonPropertyName("154")]
    public byte UpGradeMin24 { get; set; }

    [JsonPropertyName("155")]
    public byte UpGradeMax24 { get; set; }

    [JsonPropertyName("156")]
    public string Rule24 { get; set; }

    [JsonPropertyName("157")]
    public short Num24 { get; set; }

    [JsonPropertyName("158")]
    public string DrItem25 { get; set; }

    [JsonPropertyName("159")]
    public int DrItem25R { get; set; }

    [JsonPropertyName("160")]
    public byte UpGradeMin25 { get; set; }

    [JsonPropertyName("161")]
    public byte UpGradeMax25 { get; set; }

    [JsonPropertyName("162")]
    public string Rule25 { get; set; }

    [JsonPropertyName("163")]
    public short Num25 { get; set; }

    [JsonPropertyName("164")]
    public string DrItem26 { get; set; }

    [JsonPropertyName("165")]
    public int DrItem26R { get; set; }

    [JsonPropertyName("166")]
    public byte UpGradeMin26 { get; set; }

    [JsonPropertyName("167")]
    public byte UpGradeMax26 { get; set; }

    [JsonPropertyName("168")]
    public string Rule26 { get; set; }

    [JsonPropertyName("169")]
    public short Num26 { get; set; }

    [JsonPropertyName("170")]
    public string DrItem27 { get; set; }

    [JsonPropertyName("171")]
    public int DrItem27R { get; set; }

    [JsonPropertyName("172")]
    public byte UpGradeMin27 { get; set; }

    [JsonPropertyName("173")]
    public byte UpGradeMax27 { get; set; }

    [JsonPropertyName("174")]
    public string Rule27 { get; set; }

    [JsonPropertyName("175")]
    public short Num27 { get; set; }

    [JsonPropertyName("176")]
    public string DrItem28 { get; set; }

    [JsonPropertyName("177")]
    public int DrItem28R { get; set; }

    [JsonPropertyName("178")]
    public byte UpGradeMin28 { get; set; }

    [JsonPropertyName("179")]
    public byte UpGradeMax28 { get; set; }

    [JsonPropertyName("180")]
    public string Rule28 { get; set; }

    [JsonPropertyName("181")]
    public short Num28 { get; set; }

    [JsonPropertyName("182")]
    public string DrItem29 { get; set; }

    [JsonPropertyName("183")]
    public int DrItem29R { get; set; }

    [JsonPropertyName("184")]
    public byte UpGradeMin29 { get; set; }

    [JsonPropertyName("185")]
    public byte UpGradeMax29 { get; set; }

    [JsonPropertyName("186")]
    public string Rule29 { get; set; }

    [JsonPropertyName("187")]
    public short Num29 { get; set; }

    [JsonPropertyName("188")]
    public string DrItem30 { get; set; }

    [JsonPropertyName("189")]
    public int DrItem30R { get; set; }

    [JsonPropertyName("190")]
    public byte UpGradeMin30 { get; set; }

    [JsonPropertyName("191")]
    public byte UpGradeMax30 { get; set; }

    [JsonPropertyName("192")]
    public string Rule30 { get; set; }

    [JsonPropertyName("193")]
    public short Num30 { get; set; }

    [JsonPropertyName("194")]
    public string DrItem31 { get; set; }

    [JsonPropertyName("195")]
    public int DrItem31R { get; set; }

    [JsonPropertyName("196")]
    public byte UpGradeMin31 { get; set; }

    [JsonPropertyName("197")]
    public byte UpGradeMax31 { get; set; }

    [JsonPropertyName("198")]
    public string Rule31 { get; set; }

    [JsonPropertyName("199")]
    public short Num31 { get; set; }

    [JsonPropertyName("200")]
    public string DrItem32 { get; set; }

    [JsonPropertyName("201")]
    public int DrItem32R { get; set; }

    [JsonPropertyName("202")]
    public byte UpGradeMin32 { get; set; }

    [JsonPropertyName("203")]
    public byte UpGradeMax32 { get; set; }

    [JsonPropertyName("204")]
    public string Rule32 { get; set; }

    [JsonPropertyName("205")]
    public short Num32 { get; set; }

    [JsonPropertyName("206")]
    public string DrItem33 { get; set; }

    [JsonPropertyName("207")]
    public int DrItem33R { get; set; }

    [JsonPropertyName("208")]
    public byte UpGradeMin33 { get; set; }

    [JsonPropertyName("209")]
    public byte UpGradeMax33 { get; set; }

    [JsonPropertyName("210")]
    public string Rule33 { get; set; }

    [JsonPropertyName("211")]
    public short Num33 { get; set; }

    [JsonPropertyName("212")]
    public string DrItem34 { get; set; }

    [JsonPropertyName("213")]
    public int DrItem34R { get; set; }

    [JsonPropertyName("214")]
    public byte UpGradeMin34 { get; set; }

    [JsonPropertyName("215")]
    public byte UpGradeMax34 { get; set; }

    [JsonPropertyName("216")]
    public string Rule34 { get; set; }

    [JsonPropertyName("217")]
    public short Num34 { get; set; }

    [JsonPropertyName("218")]
    public string DrItem35 { get; set; }

    [JsonPropertyName("219")]
    public int DrItem35R { get; set; }

    [JsonPropertyName("220")]
    public byte UpGradeMin35 { get; set; }

    [JsonPropertyName("221")]
    public byte UpGradeMax35 { get; set; }

    [JsonPropertyName("222")]
    public string Rule35 { get; set; }

    [JsonPropertyName("223")]
    public short Num35 { get; set; }

    [JsonPropertyName("224")]
    public string DrItem36 { get; set; }

    [JsonPropertyName("225")]
    public int DrItem36R { get; set; }

    [JsonPropertyName("226")]
    public byte UpGradeMin36 { get; set; }

    [JsonPropertyName("227")]
    public byte UpGradeMax36 { get; set; }

    [JsonPropertyName("228")]
    public string Rule36 { get; set; }

    [JsonPropertyName("229")]
    public short Num36 { get; set; }

    [JsonPropertyName("230")]
    public string DrItem37 { get; set; }

    [JsonPropertyName("231")]
    public int DrItem37R { get; set; }

    [JsonPropertyName("232")]
    public byte UpGradeMin37 { get; set; }

    [JsonPropertyName("233")]
    public byte UpGradeMax37 { get; set; }

    [JsonPropertyName("234")]
    public string Rule37 { get; set; }

    [JsonPropertyName("235")]
    public short Num37 { get; set; }

    [JsonPropertyName("236")]
    public string DrItem38 { get; set; }

    [JsonPropertyName("237")]
    public int DrItem38R { get; set; }

    [JsonPropertyName("238")]
    public byte UpGradeMin38 { get; set; }

    [JsonPropertyName("239")]
    public byte UpGradeMax38 { get; set; }

    [JsonPropertyName("240")]
    public string Rule38 { get; set; }

    [JsonPropertyName("241")]
    public short Num38 { get; set; }

    [JsonPropertyName("242")]
    public string DrItem39 { get; set; }

    [JsonPropertyName("243")]
    public int DrItem39R { get; set; }

    [JsonPropertyName("244")]
    public byte UpGradeMin39 { get; set; }

    [JsonPropertyName("245")]
    public byte UpGradeMax39 { get; set; }

    [JsonPropertyName("246")]
    public string Rule39 { get; set; }

    [JsonPropertyName("247")]
    public short Num39 { get; set; }

    [JsonPropertyName("248")]
    public string DrItem40 { get; set; }

    [JsonPropertyName("249")]
    public int DrItem40R { get; set; }

    [JsonPropertyName("250")]
    public byte UpGradeMin40 { get; set; }

    [JsonPropertyName("251")]
    public byte UpGradeMax40 { get; set; }

    [JsonPropertyName("252")]
    public string Rule40 { get; set; }

    [JsonPropertyName("253")]
    public short Num40 { get; set; }

    [JsonPropertyName("254")]
    public string DrItem41 { get; set; }

    [JsonPropertyName("255")]
    public int DrItem41R { get; set; }

    [JsonPropertyName("256")]
    public byte UpGradeMin41 { get; set; }

    [JsonPropertyName("257")]
    public byte UpGradeMax41 { get; set; }

    [JsonPropertyName("258")]
    public string Rule41 { get; set; }

    [JsonPropertyName("259")]
    public short Num41 { get; set; }

    [JsonPropertyName("260")]
    public string DrItem42 { get; set; }

    [JsonPropertyName("261")]
    public int DrItem42R { get; set; }

    [JsonPropertyName("262")]
    public byte UpGradeMin42 { get; set; }

    [JsonPropertyName("263")]
    public byte UpGradeMax42 { get; set; }

    [JsonPropertyName("264")]
    public string Rule42 { get; set; }

    [JsonPropertyName("265")]
    public short Num42 { get; set; }

    [JsonPropertyName("266")]
    public string DrItem43 { get; set; }

    [JsonPropertyName("267")]
    public int DrItem43R { get; set; }

    [JsonPropertyName("268")]
    public byte UpGradeMin43 { get; set; }

    [JsonPropertyName("269")]
    public byte UpGradeMax43 { get; set; }

    [JsonPropertyName("270")]
    public string Rule43 { get; set; }

    [JsonPropertyName("271")]
    public short Num43 { get; set; }

    [JsonPropertyName("272")]
    public string DrItem44 { get; set; }

    [JsonPropertyName("273")]
    public int DrItem44R { get; set; }

    [JsonPropertyName("274")]
    public byte UpGradeMin44 { get; set; }

    [JsonPropertyName("275")]
    public byte UpGradeMax44 { get; set; }

    [JsonPropertyName("276")]
    public string Rule44 { get; set; }

    [JsonPropertyName("277")]
    public short Num44 { get; set; }

    [JsonPropertyName("278")]
    public string DrItem45 { get; set; }

    [JsonPropertyName("279")]
    public int DrItem45R { get; set; }

    [JsonPropertyName("280")]
    public byte UpGradeMin45 { get; set; }

    [JsonPropertyName("281")]
    public byte UpGradeMax45 { get; set; }

    [JsonPropertyName("282")]
    public string Rule45 { get; set; }

    [JsonPropertyName("283")]
    public short Num45 { get; set; }

    [JsonPropertyName("284")]
    public string ExcItem1 { get; set; }

    [JsonPropertyName("285")]
    public string ExcItem2 { get; set; }

    [JsonPropertyName("286")]
    public string ExcItem3 { get; set; }

    [JsonPropertyName("287")]
    public string ExcItem4 { get; set; }

    [JsonPropertyName("288")]
    public string ExcItem5 { get; set; }

    [JsonPropertyName("289")]
    public int CheckSum { get; set; }
}