using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelSiteParser
{
    public enum CodePage
    {
        /// <summary>Default system encoding</summary>
        [Description("Default system encoding")]
        Default = 0,
        /// <summary>IBM EBCDIC (US-Canada)</summary>
        [DescriptionAttribute("IBM EBCDIC (US-Canada)")]
        IBM037 = 37,
        /// <summary>OEM United States</summary>
        [DescriptionAttribute("OEM United States")]
        IBM437 = 437,
        /// <summary>IBM EBCDIC (International)</summary>
        [DescriptionAttribute("IBM EBCDIC (International)")]
        IBM500 = 500,
        /// <summary>Arabic (ASMO 708)</summary>
        [DescriptionAttribute("Arabic (ASMO 708)")]
        ASMO708 = 708,
        /// <summary>Arabic (DOS)</summary>
        [DescriptionAttribute("Arabic (DOS)")]
        DOS720 = 720,
        /// <summary>Greek (DOS)</summary>
        [DescriptionAttribute("Greek (DOS)")]
        Ibm737 = 737,
        /// <summary>Baltic (DOS)</summary>
        [DescriptionAttribute("Baltic (DOS)")]
        Ibm775 = 775,
        /// <summary>Western European (DOS)</summary>
        [DescriptionAttribute("Western European (DOS)")]
        Ibm850 = 850,
        /// <summary>Central European (DOS)</summary>
        [DescriptionAttribute("Central European (DOS)")]
        Ibm852 = 852,
        /// <summary>OEM Cyrillic</summary>
        [DescriptionAttribute("OEM Cyrillic")]
        IBM855 = 855,
        /// <summary>Turkish (DOS)</summary>
        [DescriptionAttribute("Turkish (DOS)")]
        Ibm857 = 857,
        /// <summary>OEM Multilingual Latin I</summary>
        [DescriptionAttribute("OEM Multilingual Latin I")]
        IBM00858 = 858,
        /// <summary>Portuguese (DOS)</summary>
        [DescriptionAttribute("Portuguese (DOS)")]
        IBM860 = 860,
        /// <summary>Icelandic (DOS)</summary>
        [DescriptionAttribute("Icelandic (DOS)")]
        Ibm861 = 861,
        /// <summary>Hebrew (DOS)</summary>
        [DescriptionAttribute("Hebrew (DOS)")]
        DOS862 = 862,
        /// <summary>French Canadian (DOS)</summary>
        [DescriptionAttribute("French Canadian (DOS)")]
        IBM863 = 863,
        /// <summary>Arabic (864)</summary>
        [DescriptionAttribute("Arabic (864)")]
        IBM864 = 864,
        /// <summary>Nordic (DOS)</summary>
        [DescriptionAttribute("Nordic (DOS)")]
        IBM865 = 865,
        /// <summary>Cyrillic (DOS)</summary>
        [DescriptionAttribute("Cyrillic (DOS)")]
        Cp866 = 866,
        /// <summary>Greek, Modern (DOS)</summary>
        [DescriptionAttribute("Greek, Modern (DOS)")]
        Ibm869 = 869,
        /// <summary>IBM EBCDIC (Multilingual Latin-2)</summary>
        [DescriptionAttribute("IBM EBCDIC (Multilingual Latin-2)")]
        IBM870 = 870,
        /// <summary>Thai (Windows)</summary>
        [DescriptionAttribute("Thai (Windows)")]
        Windows874 = 874,
        /// <summary>IBM EBCDIC (Greek Modern)</summary>
        [DescriptionAttribute("IBM EBCDIC (Greek Modern)")]
        Cp875 = 875,
        /// <summary>Japanese (Shift-JIS)</summary>
        [DescriptionAttribute("Japanese (Shift-JIS)")]
        Shiftjis = 932,
        /// <summary>Chinese Simplified (GB2312)</summary>
        [DescriptionAttribute("Chinese Simplified (GB2312)")]
        Gb2312 = 936,
        /// <summary>Korean</summary>
        [DescriptionAttribute("Korean")]
        Ksc56011987 = 949,
        /// <summary>Chinese Traditional (Big5)</summary>
        [DescriptionAttribute("Chinese Traditional (Big5)")]
        Big5 = 950,
        /// <summary>IBM EBCDIC (Turkish Latin-5)</summary>
        [DescriptionAttribute("IBM EBCDIC (Turkish Latin-5)")]
        IBM1026 = 1026,
        /// <summary>IBM Latin-1</summary>
        [DescriptionAttribute("IBM Latin-1")]
        IBM01047 = 1047,
        /// <summary>IBM EBCDIC (US-Canada-Euro)</summary>
        [DescriptionAttribute("IBM EBCDIC (US-Canada-Euro)")]
        IBM01140 = 1140,
        /// <summary>IBM EBCDIC (Germany-Euro)</summary>
        [DescriptionAttribute("IBM EBCDIC (Germany-Euro)")]
        IBM01141 = 1141,
        /// <summary>IBM EBCDIC (Denmark-Norway-Euro)</summary>
        [DescriptionAttribute("IBM EBCDIC (Denmark-Norway-Euro)")]
        IBM01142 = 1142,
        /// <summary>IBM EBCDIC (Finland-Sweden-Euro)</summary>
        [DescriptionAttribute("IBM EBCDIC (Finland-Sweden-Euro)")]
        IBM01143 = 1143,
        /// <summary>IBM EBCDIC (Italy-Euro)</summary>
        [DescriptionAttribute("IBM EBCDIC (Italy-Euro)")]
        IBM01144 = 1144,
        /// <summary>IBM EBCDIC (Spain-Euro)</summary>
        [DescriptionAttribute("IBM EBCDIC (Spain-Euro)")]
        IBM01145 = 1145,
        /// <summary>IBM EBCDIC (UK-Euro)</summary>
        [DescriptionAttribute("IBM EBCDIC (UK-Euro)")]
        IBM01146 = 1146,
        /// <summary>IBM EBCDIC (France-Euro)</summary>
        [DescriptionAttribute("IBM EBCDIC (France-Euro)")]
        IBM01147 = 1147,
        /// <summary>IBM EBCDIC (International-Euro)</summary>
        [DescriptionAttribute("IBM EBCDIC (International-Euro)")]
        IBM01148 = 1148,
        /// <summary>IBM EBCDIC (Icelandic-Euro)</summary>
        [DescriptionAttribute("IBM EBCDIC (Icelandic-Euro)")]
        IBM01149 = 1149,
        /// <summary>Unicode</summary>
        [DescriptionAttribute("Unicode")]
        Utf16 = 1200,
        /// <summary>Unicode (Big endian)</summary>
        [DescriptionAttribute("Unicode (Big endian)")]
        UnicodeFFFE = 1201,
        /// <summary>Central European (Windows)</summary>
        [DescriptionAttribute("Central European (Windows)")]
        Windows1250 = 1250,
        /// <summary>Cyrillic (Windows)</summary>
        [DescriptionAttribute("Cyrillic (Windows)")]
        Windows1251 = 1251,
        /// <summary>Western European (Windows)</summary>
        [DescriptionAttribute("Western European (Windows)")]
        Windows1252 = 1252,
        /// <summary>Greek (Windows)</summary>
        [DescriptionAttribute("Greek (Windows)")]
        Windows1253 = 1253,
        /// <summary>Turkish (Windows)</summary>
        [DescriptionAttribute("Turkish (Windows)")]
        Windows1254 = 1254,
        /// <summary>Hebrew (Windows)</summary>
        [DescriptionAttribute("Hebrew (Windows)")]
        Windows1255 = 1255,
        /// <summary>Arabic (Windows)</summary>
        [DescriptionAttribute("Arabic (Windows)")]
        Windows1256 = 1256,
        /// <summary>Baltic (Windows)</summary>
        [DescriptionAttribute("Baltic (Windows)")]
        Windows1257 = 1257,
        /// <summary>Vietnamese (Windows)</summary>
        [DescriptionAttribute("Vietnamese (Windows)")]
        Windows1258 = 1258,
        /// <summary>Korean (Johab)</summary>
        [DescriptionAttribute("Korean (Johab)")]
        Johab = 1361,
        /// <summary>Western European (Mac)</summary>
        [DescriptionAttribute("Western European (Mac)")]
        Macintosh = 10000,
        /// <summary>Japanese (Mac)</summary>
        [DescriptionAttribute("Japanese (Mac)")]
        Xmacjapanese = 10001,
        /// <summary>Chinese Traditional (Mac)</summary>
        [DescriptionAttribute("Chinese Traditional (Mac)")]
        Xmacchinesetrad = 10002,
        /// <summary>Korean (Mac)</summary>
        [DescriptionAttribute("Korean (Mac)")]
        Xmackorean = 10003,
        /// <summary>Arabic (Mac)</summary>
        [DescriptionAttribute("Arabic (Mac)")]
        Xmacarabic = 10004,
        /// <summary>Hebrew (Mac)</summary>
        [DescriptionAttribute("Hebrew (Mac)")]
        Xmachebrew = 10005,
        /// <summary>Greek (Mac)</summary>
        [DescriptionAttribute("Greek (Mac)")]
        Xmacgreek = 10006,
        /// <summary>Cyrillic (Mac)</summary>
        [DescriptionAttribute("Cyrillic (Mac)")]
        Xmaccyrillic = 10007,
        /// <summary>Chinese Simplified (Mac)</summary>
        [DescriptionAttribute("Chinese Simplified (Mac)")]
        Xmacchinesesimp = 10008,
        /// <summary>Romanian (Mac)</summary>
        [DescriptionAttribute("Romanian (Mac)")]
        Xmacromanian = 10010,
        /// <summary>Ukrainian (Mac)</summary>
        [DescriptionAttribute("Ukrainian (Mac)")]
        Xmacukrainian = 10017,
        /// <summary>Thai (Mac)</summary>
        [DescriptionAttribute("Thai (Mac)")]
        Xmacthai = 10021,
        /// <summary>Central European (Mac)</summary>
        [DescriptionAttribute("Central European (Mac)")]
        Xmacce = 10029,
        /// <summary>Icelandic (Mac)</summary>
        [DescriptionAttribute("Icelandic (Mac)")]
        Xmacicelandic = 10079,
        /// <summary>Turkish (Mac)</summary>
        [DescriptionAttribute("Turkish (Mac)")]
        Xmacturkish = 10081,
        /// <summary>Croatian (Mac)</summary>
        [DescriptionAttribute("Croatian (Mac)")]
        Xmaccroatian = 10082,
        /// <summary>Unicode (UTF-32)</summary>
        [DescriptionAttribute("Unicode (UTF-32)")]
        Utf32 = 12000,
        /// <summary>Unicode (UTF-32 Big endian)</summary>
        [DescriptionAttribute("Unicode (UTF-32 Big endian)")]
        Utf32BE = 12001,
        /// <summary>Chinese Traditional (CNS)</summary>
        [DescriptionAttribute("Chinese Traditional (CNS)")]
        XChineseCNS = 20000,
        /// <summary>TCA Taiwan</summary>
        [DescriptionAttribute("TCA Taiwan")]
        Xcp20001 = 20001,
        /// <summary>Chinese Traditional (Eten)</summary>
        [DescriptionAttribute("Chinese Traditional (Eten)")]
        XChineseEten = 20002,
        /// <summary>IBM5550 Taiwan</summary>
        [DescriptionAttribute("IBM5550 Taiwan")]
        Xcp20003 = 20003,
        /// <summary>TeleText Taiwan</summary>
        [DescriptionAttribute("TeleText Taiwan")]
        Xcp20004 = 20004,
        /// <summary>Wang Taiwan</summary>
        [DescriptionAttribute("Wang Taiwan")]
        Xcp20005 = 20005,
        /// <summary>Western European (IA5)</summary>
        [DescriptionAttribute("Western European (IA5)")]
        XIA5 = 20105,
        /// <summary>German (IA5)</summary>
        [DescriptionAttribute("German (IA5)")]
        XIA5German = 20106,
        /// <summary>Swedish (IA5)</summary>
        [DescriptionAttribute("Swedish (IA5)")]
        XIA5Swedish = 20107,
        /// <summary>Norwegian (IA5)</summary>
        [DescriptionAttribute("Norwegian (IA5)")]
        XIA5Norwegian = 20108,
        /// <summary>US-ASCII</summary>
        [DescriptionAttribute("US-ASCII")]
        UsAscii = 20127,
        /// <summary>T.61</summary>
        [DescriptionAttribute("T.61")]
        Xcp20261 = 20261,
        /// <summary>ISO-6937</summary>
        [DescriptionAttribute("ISO-6937")]
        Xcp20269 = 20269,
        /// <summary>IBM EBCDIC (Germany)</summary>
        [DescriptionAttribute("IBM EBCDIC (Germany)")]
        IBM273 = 20273,
        /// <summary>IBM EBCDIC (Denmark-Norway)</summary>
        [DescriptionAttribute("IBM EBCDIC (Denmark-Norway)")]
        IBM277 = 20277,
        /// <summary>IBM EBCDIC (Finland-Sweden)</summary>
        [DescriptionAttribute("IBM EBCDIC (Finland-Sweden)")]
        IBM278 = 20278,
        /// <summary>IBM EBCDIC (Italy)</summary>
        [DescriptionAttribute("IBM EBCDIC (Italy)")]
        IBM280 = 20280,
        /// <summary>IBM EBCDIC (Spain)</summary>
        [DescriptionAttribute("IBM EBCDIC (Spain)")]
        IBM284 = 20284,
        /// <summary>IBM EBCDIC (UK)</summary>
        [DescriptionAttribute("IBM EBCDIC (UK)")]
        IBM285 = 20285,
        /// <summary>IBM EBCDIC (Japanese katakana)</summary>
        [DescriptionAttribute("IBM EBCDIC (Japanese katakana)")]
        IBM290 = 20290,
        /// <summary>IBM EBCDIC (France)</summary>
        [DescriptionAttribute("IBM EBCDIC (France)")]
        IBM297 = 20297,
        /// <summary>IBM EBCDIC (Arabic)</summary>
        [DescriptionAttribute("IBM EBCDIC (Arabic)")]
        IBM420 = 20420,
        /// <summary>IBM EBCDIC (Greek)</summary>
        [DescriptionAttribute("IBM EBCDIC (Greek)")]
        IBM423 = 20423,
        /// <summary>IBM EBCDIC (Hebrew)</summary>
        [DescriptionAttribute("IBM EBCDIC (Hebrew)")]
        IBM424 = 20424,
        /// <summary>IBM EBCDIC (Korean Extended)</summary>
        [DescriptionAttribute("IBM EBCDIC (Korean Extended)")]
        XEBCDICKoreanExtended = 20833,
        /// <summary>IBM EBCDIC (Thai)</summary>
        [DescriptionAttribute("IBM EBCDIC (Thai)")]
        IBMThai = 20838,
        /// <summary>Cyrillic (KOI8-R)</summary>
        [DescriptionAttribute("Cyrillic (KOI8-R)")]
        Koi8r = 20866,
        /// <summary>IBM EBCDIC (Icelandic)</summary>
        [DescriptionAttribute("IBM EBCDIC (Icelandic)")]
        IBM871 = 20871,
        /// <summary>IBM EBCDIC (Cyrillic Russian)</summary>
        [DescriptionAttribute("IBM EBCDIC (Cyrillic Russian)")]
        IBM880 = 20880,
        /// <summary>IBM EBCDIC (Turkish)</summary>
        [DescriptionAttribute("IBM EBCDIC (Turkish)")]
        IBM905 = 20905,
        /// <summary>IBM Latin-1</summary>
        [DescriptionAttribute("IBM Latin-1")]
        IBM00924 = 20924,
        /// <summary>Japanese (JIS 0208-1990 and 0212-1990)</summary>
        [DescriptionAttribute("Japanese (JIS 0208-1990 and 0212-1990)")]
        EUCJP_1990 = 20932,
        /// <summary>Chinese Simplified (GB2312-80)</summary>
        [DescriptionAttribute("Chinese Simplified (GB2312-80)")]
        Xcp20936 = 20936,
        /// <summary>Korean Wansung</summary>
        [DescriptionAttribute("Korean Wansung")]
        Xcp20949 = 20949,
        /// <summary>IBM EBCDIC (Cyrillic Serbian-Bulgarian)</summary>
        [DescriptionAttribute("IBM EBCDIC (Cyrillic Serbian-Bulgarian)")]
        Cp1025 = 21025,
        /// <summary>Cyrillic (KOI8-U)</summary>
        [DescriptionAttribute("Cyrillic (KOI8-U)")]
        Koi8u = 21866,
        /// <summary>Western European (ISO)</summary>
        [DescriptionAttribute("Western European (ISO)")]
        Iso88591 = 28591,
        /// <summary>Central European (ISO)</summary>
        [DescriptionAttribute("Central European (ISO)")]
        Iso88592 = 28592,
        /// <summary>Latin 3 (ISO)</summary>
        [DescriptionAttribute("Latin 3 (ISO)")]
        Iso88593 = 28593,
        /// <summary>Baltic (ISO)</summary>
        [DescriptionAttribute("Baltic (ISO)")]
        Iso88594 = 28594,
        /// <summary>Cyrillic (ISO)</summary>
        [DescriptionAttribute("Cyrillic (ISO)")]
        Iso88595 = 28595,
        /// <summary>Arabic (ISO)</summary>
        [DescriptionAttribute("Arabic (ISO)")]
        Iso88596 = 28596,
        /// <summary>Greek (ISO)</summary>
        [DescriptionAttribute("Greek (ISO)")]
        Iso88597 = 28597,
        /// <summary>Hebrew (ISO-Visual)</summary>
        [DescriptionAttribute("Hebrew (ISO-Visual)")]
        Iso88598 = 28598,
        /// <summary>Turkish (ISO)</summary>
        [DescriptionAttribute("Turkish (ISO)")]
        Iso88599 = 28599,
        /// <summary>Estonian (ISO)</summary>
        [DescriptionAttribute("Estonian (ISO)")]
        Iso885913 = 28603,
        /// <summary>Latin 9 (ISO)</summary>
        [DescriptionAttribute("Latin 9 (ISO)")]
        Iso885915 = 28605,
        /// <summary>Europa</summary>
        [DescriptionAttribute("Europa")]
        XEuropa = 29001,
        /// <summary>Hebrew (ISO-Logical)</summary>
        [DescriptionAttribute("Hebrew (ISO-Logical)")]
        Iso88598i = 38598,
        /// <summary>Japanese (JIS)</summary>
        [DescriptionAttribute("Japanese (JIS)")]
        Iso2022jp = 50220,
        /// <summary>Japanese (JIS-Allow 1 byte Kana)</summary>
        [DescriptionAttribute("Japanese (JIS-Allow 1 byte Kana)")]
        CsISO2022JP = 50221,
        /// <summary>Japanese (JIS-Allow 1 byte Kana - SO/SI)</summary>
        [DescriptionAttribute("Japanese (JIS-Allow 1 byte Kana - SO/SI)")]
        Iso2022jpOneByte = 50222,
        /// <summary>Korean (ISO)</summary>
        [DescriptionAttribute("Korean (ISO)")]
        Iso2022kr = 50225,
        /// <summary>Chinese Simplified (ISO-2022)</summary>
        [DescriptionAttribute("Chinese Simplified (ISO-2022)")]
        Xcp50227 = 50227,
        /// <summary>Japanese (EUC)</summary>
        [DescriptionAttribute("Japanese (EUC)")]
        Eucjp = 51932,
        /// <summary>Chinese Simplified (EUC)</summary>
        [DescriptionAttribute("Chinese Simplified (EUC)")]
        EUCCN = 51936,
        /// <summary>Korean (EUC)</summary>
        [DescriptionAttribute("Korean (EUC)")]
        Euckr = 51949,
        /// <summary>Chinese Simplified (HZ)</summary>
        [DescriptionAttribute("Chinese Simplified (HZ)")]
        Hzgb2312 = 52936,
        /// <summary>Chinese Simplified (GB18030)</summary>
        [DescriptionAttribute("Chinese Simplified (GB18030)")]
        GB18030 = 54936,
        /// <summary>ISCII Devanagari</summary>
        [DescriptionAttribute("ISCII Devanagari")]
        Xisciide = 57002,
        /// <summary>ISCII Bengali</summary>
        [DescriptionAttribute("ISCII Bengali")]
        Xisciibe = 57003,
        /// <summary>ISCII Tamil</summary>
        [DescriptionAttribute("ISCII Tamil")]
        Xisciita = 57004,
        /// <summary>ISCII Telugu</summary>
        [DescriptionAttribute("ISCII Telugu")]
        Xisciite = 57005,
        /// <summary>ISCII Assamese</summary>
        [DescriptionAttribute("ISCII Assamese")]
        Xisciias = 57006,
        /// <summary>ISCII Oriya</summary>
        [DescriptionAttribute("ISCII Oriya")]
        Xisciior = 57007,
        /// <summary>ISCII Kannada</summary>
        [DescriptionAttribute("ISCII Kannada")]
        Xisciika = 57008,
        /// <summary>ISCII Malayalam</summary>
        [DescriptionAttribute("ISCII Malayalam")]
        Xisciima = 57009,
        /// <summary>ISCII Gujarati</summary>
        [DescriptionAttribute("ISCII Gujarati")]
        Xisciigu = 57010,
        /// <summary>ISCII Punjabi</summary>
        [DescriptionAttribute("ISCII Punjabi")]
        Xisciipa = 57011,
        /// <summary>Unicode (UTF-7)</summary>
        [DescriptionAttribute("Unicode (UTF-7)")]
        Utf7 = 65000,
        /// <summary>Unicode (UTF-8)</summary>
        [DescriptionAttribute("Unicode (UTF-8)")]
        Utf8 = 65001
    }
}
