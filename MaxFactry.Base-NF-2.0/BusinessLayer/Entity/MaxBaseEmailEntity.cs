// <copyright file="MaxBaseEmailEntity.cs" company="Lakstins Family, LLC">
// Copyright (c) Brian A. Lakstins (http://www.lakstins.com/brian/)
// </copyright>

#region License
// <license>
// This software is provided 'as-is', without any express or implied warranty. In no 
// event will the author be held liable for any damages arising from the use of this 
// software.
//  
// Permission is granted to anyone to use this software for any purpose, including 
// commercial applications, and to alter it and redistribute it freely, subject to the 
// following restrictions:
// 
// 1. The origin of this software must not be misrepresented; you must not claim that 
// you wrote the original software. If you use this software in a product, an 
// acknowledgment (see the following) in the product documentation is required.
// 
// Portions Copyright (c) Brian A. Lakstins (http://www.lakstins.com/brian/)
// 
// 2. Altered source versions must be plainly marked as such, and must not be 
// misrepresented as being the original software.
// 
// 3. This notice may not be removed or altered from any source distribution.
// </license>
#endregion

#region Change Log
// <changelog>
// <change date="4/9/2015" author="Brian A. Lakstins" description="Initial creation">
// <change date="12/18/2015" author="Brian A. Lakstins" description="Added a way to incude attachments">
// <change date="8/25/2020" author="Brian A. Lakstins" description="Change email valid check to static.  Update TLD domain name list.">
// <change date="9/17/2020" author="Brian A. Lakstins" description="Automatically up date content with Id of the email">
// <change date="9/24/2020" author="Brian A. Lakstins" description="Update handling of multiple to addresses and names">
// <change date="12/1/2020" author="Brian A. Lakstins" description="Store to email address as lowercase">
// <change date="12/10/2020" author="Brian A. Lakstins" description="Add SentCount.  Remove Moustache handling of Id. Using Nustache library before sending.">
// <change date="5/18/2021" author="Brian A. Lakstins" description="Update load by relation to use loadbypropertcache method.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Updated for changes namespaces">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;
    using System.Collections.Generic;

    /// <summary>
    /// Entity to represent virtual text file in a web site.
    /// </summary>
    public abstract class MaxBaseEmailEntity : MaxBaseGuidKeyEntity
    {
        private List<string> _oToAddressList = null;

        private List<string> _oToNameList = null;

        /// <summary>
        /// Initializes a new instance of the MaxBaseEmailEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxBaseEmailEntity(MaxData loData)
            : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseEmailEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseEmailEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets or sets the content of the email
        /// </summary>
        public string Content
        {
            get
            {
                return this.GetString(this.EmailDataModel.Content);
            }

            set
            {
                this.Set(this.EmailDataModel.Content, value);
            }
        }

        /// <summary>
        /// Gets or sets the address the email is sent from
        /// </summary>
        public string FromAddress
        {
            get
            {
                return this.GetString(this.EmailDataModel.FromAddress);
            }

            set
            {
                this.Set(this.EmailDataModel.FromAddress, value);
            }
        }

        /// <summary>
        /// Gets or sets the name the email is sent from
        /// </summary>
        public string FromName
        {
            get
            {
                return this.GetString(this.EmailDataModel.FromName);
            }

            set
            {
                this.Set(this.EmailDataModel.FromName, value);
            }
        }

        /// <summary>
        /// Gets or sets the relation Id for the email (all emails are related to something).
        /// </summary>
        public Guid RelationId
        {
            get
            {
                return this.GetGuid(this.EmailDataModel.RelationId);
            }

            set
            {
                this.Set(this.EmailDataModel.RelationId, value);
            }
        }

        /// <summary>
        /// Gets or sets the relation type.  Also used to get configuration for sending email.
        /// </summary>
        public string RelationType
        {
            get
            {
                return this.GetString(this.EmailDataModel.RelationType);
            }

            set
            {
                this.Set(this.EmailDataModel.RelationType, value);
            }
        }

        /// <summary>
        /// Gets or sets the subject of the email
        /// </summary>
        public string Subject
        {
            get
            {
                return this.GetString(this.EmailDataModel.Subject);
            }

            set
            {
                this.Set(this.EmailDataModel.Subject, value);
            }
        }

        /// <summary>
        /// Gets or sets the text list of addresses to send the email to
        /// </summary>
        public string ToAddressListText
        {
            get
            {
                return this.GetString(this.EmailDataModel.ToAddressList);
            }

            set
            {
                this.Set(this.EmailDataModel.ToAddressList, value);
                this._oToAddressList = null;
            }
        }

        /// <summary>
        /// Gets or sets the list of addresses to send the email to
        /// </summary>
        public List<string> ToAddressList
        {
            get
            {
                if (null == _oToAddressList)
                {
                    string lsToAddressList = this.ToAddressListText;
                    string[] laToAddressList = lsToAddressList.Split('\t');
                    if (laToAddressList.Length == 1)
                    {
                        string lsAddressListParsed = lsToAddressList.Replace("\";\"", "\t");
                        if (lsAddressListParsed.IndexOf(';') != -1)
                        {
                            laToAddressList = lsAddressListParsed.Split(';');
                            for (int lnN = 0; lnN < laToAddressList.Length; lnN++)
                            {
                                laToAddressList[lnN] = laToAddressList[lnN].Replace("\t", "\";\"");
                            }
                        }

                        if (laToAddressList.Length == 1)
                        {
                            lsAddressListParsed = lsToAddressList.Replace("\",\"", "\t");
                            if (lsAddressListParsed.IndexOf(',') != -1)
                            {
                                laToAddressList = lsAddressListParsed.Split(',');
                                for (int lnN = 0; lnN < laToAddressList.Length; lnN++)
                                {
                                    laToAddressList[lnN] = laToAddressList[lnN].Replace("\t", "\",\"");
                                }
                            }
                        }
                    }

                    for (int lnN = 0; lnN < laToAddressList.Length; lnN++)
                    {
                        laToAddressList[lnN] = laToAddressList[lnN].ToLower();
                    }

                    this._oToAddressList = new List<string>(laToAddressList);
                }

                return this._oToAddressList;
            }

            set
            {
                this._oToAddressList = value;
                string lsListText = string.Empty;
                foreach (string lsText in this._oToAddressList)
                {
                    lsListText += lsText.ToLowerInvariant() + "\t";
                }

                this.Set(this.EmailDataModel.ToAddressList, lsListText.Substring(0, lsListText.Length - 1));
            }
        }

        /// <summary>
        /// Gets or sets the list of names to send the email to
        /// </summary>
        public string ToNameListText
        {
            get
            {
                return this.GetString(this.EmailDataModel.ToNameList);
            }

            set
            {
                this.Set(this.EmailDataModel.ToNameList, value);
                this._oToNameList = null;
            }
        }

        /// <summary>
        /// Gets or sets the list of names to send the email to
        /// </summary>
        public List<string> ToNameList
        {
            get
            {
                if (null == _oToNameList)
                {
                    string[] laToNameList = new string[] { string.Empty };
                    string lsToNameList = this.ToNameListText;
                    if (lsToNameList.Length > 0)
                    {
                        laToNameList = lsToNameList.Split('\t');
                    }

                    this._oToNameList = new List<string>(laToNameList);
                }

                return this._oToNameList;
            }

            set
            {
                this._oToNameList = value;
                string lsListText = string.Empty;
                foreach (string lsText in this._oToNameList)
                {
                    lsListText += lsText + "\t";
                }

                this.Set(this.EmailDataModel.ToNameList, lsListText.Substring(0, lsListText.Length - 1));
            }
        }

        /// <summary>
        /// Gets or sets the group of emails that this email belongs to.
        /// </summary>
        public string GroupName
        {
            get
            {
                return this.GetString(this.EmailDataModel.GroupName);
            }

            set
            {
                this.Set(this.EmailDataModel.GroupName, value);
            }
        }

        /// <summary>
        /// Gets or sets the group of emails that this email belongs to.
        /// </summary>
        public int AttachmentCount
        {
            get
            {
                return this.GetInt(this.EmailDataModel.AttachmentCount);
            }

            set
            {
                this.Set(this.EmailDataModel.AttachmentCount, value);
            }
        }

        /// <summary>
        /// Gets or sets the address the email should be replied to
        /// </summary>
        public string ReplyAddress
        {
            get
            {
                return this.GetString(this.EmailDataModel.ReplyAddress);
            }

            set
            {
                this.Set(this.EmailDataModel.ReplyAddress, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of emails sent
        /// </summary>
        public int SentCount
        {
            get
            {
                return this.GetInt(this.EmailDataModel.SentCount);
            }

            set
            {
                this.Set(this.EmailDataModel.SentCount, value);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseEmailDataModel EmailDataModel
        {
            get
            {
                return (MaxBaseEmailDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        protected override void SetProperties()
        {
            this.ToAddressList = this.ToAddressList;
            this.ToNameList = this.ToNameList;
            base.SetProperties();
        }

        /// <summary>
        /// Adds a file attachment to an email
        /// </summary>
        /// <param name="loStream">File stream content</param>
        /// <param name="lsName">Name of the file</param>
        /// <param name="lsMime">Mime type of the file</param>
        public virtual void AddAttachment(Stream loStream, string lsName, string lsMime)
        {
            if (this.AttachmentCount < 0)
            {
                this.AttachmentCount = 0;
            }

            MaxIndex loData = new MaxIndex();
            loData.Add("stream", loStream);
            loData.Add("name", lsName);
            loData.Add("mime", lsMime);
            this.Set(this.EmailDataModel.AttachmentCount + "_" + this.AttachmentCount, loData);
            this.AttachmentCount++;
        }

        /// <summary>
        /// Loads all based on the Relation Id
        /// </summary>
        /// <param name="loRelationId">The Relation Id to match</param>
        /// <returns>List of entities</returns>
        public virtual MaxEntityList LoadAllByRelationIdCache(Guid loRelationId)
        {
            return this.LoadAllByPropertyCache(this.EmailDataModel.RelationId, loRelationId);
        }

        /// <summary>
        /// Sends the email using the MaxMessageRepository
        /// </summary>
        public virtual void Send()
        {
            this.ToAddressList = this.ToAddressList;
            this.ToNameList = this.ToNameList;
            if (null == this.FromAddress || this.FromAddress.Length.Equals(0))
            {
                this.FromAddress = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeAny, this.RelationType + "EmailFromAddress") as string;
                if (null == this.FromAddress || this.FromAddress.Length.Equals(0))
                {
                    this.FromAddress = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeAny, "MaxDefaultEmailFromAddress") as string;
                }
            }

            if (null == this.ToAddressListText || this.ToAddressListText.Length.Equals(0))
            {
                this.ToAddressListText = MaxMessageRepository.GetConfig("EmailToAddress", this.FromAddress, this.RelationType);
            }

            if (null == this.FromName || this.FromName.Length.Equals(0))
            {
                this.FromName = MaxMessageRepository.GetConfig("EmailFromName", this.FromAddress, this.RelationType);
            }

            MaxData loData = this.Data.Clone();
            loData.Set(this.EmailDataModel.Content, this.Content);
            loData = MaxFactry.Base.DataLayer.MaxMessageRepository.Send(loData);
            this.Load(loData);
        }

        /// <summary>
        /// Checks to see if an email address is validly formatted
        /// </summary>
        /// <param name="lsEmail">Email address to check.</param>
        /// <returns>true if valid.</returns>
        public static bool IsValidEmail(string lsEmail)
        {
            if (null == lsEmail || string.Empty == lsEmail)
            {
                return false;
            }
            else if (lsEmail.Length > 254)
            {
                return false;
            }
            else
            {
                Regex loRegexIsInternetEmailAddressSimple = new Regex(@"(@)(.+)$", RegexOptions.IgnoreCase);
                if (!loRegexIsInternetEmailAddressSimple.IsMatch(lsEmail))
                {
                    return false;
                }

                string lsEmailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*";
                //// http://data.iana.org/TLD/tlds-alpha-by-domain.txt
                //// # Version 2020082400, Last Updated Mon Aug 24 07:07:01 2020 UTC
                string lsTLDList = "(?:AAA|AARP|ABARTH|ABB|ABBOTT|ABBVIE|ABC|ABLE|ABOGADO|ABUDHABI|AC|ACADEMY|ACCENTURE|ACCOUNTANT|ACCOUNTANTS|ACO|ACTOR|AD|ADAC|ADS|ADULT|AE|AEG|AERO|AETNA|AF|AFAMILYCOMPANY|AFL|AFRICA|AG|AGAKHAN|AGENCY|AI|AIG|AIRBUS|AIRFORCE|AIRTEL|AKDN|AL|ALFAROMEO|ALIBABA|ALIPAY|ALLFINANZ|ALLSTATE|ALLY|ALSACE|ALSTOM|AM|AMAZON|AMERICANEXPRESS|AMERICANFAMILY|AMEX|AMFAM|AMICA|AMSTERDAM|ANALYTICS|ANDROID|ANQUAN|ANZ|AO|AOL|APARTMENTS|APP|APPLE|AQ|AQUARELLE|AR|ARAB|ARAMCO|ARCHI|ARMY|ARPA|ART|ARTE|AS|ASDA|ASIA|ASSOCIATES|AT|ATHLETA|ATTORNEY|AU|AUCTION|AUDI|AUDIBLE|AUDIO|AUSPOST|AUTHOR|AUTO|AUTOS|AVIANCA|AW|AWS|AX|AXA|AZ|AZURE|BA|BABY|BAIDU|BANAMEX|BANANAREPUBLIC|BAND|BANK|BAR|BARCELONA|BARCLAYCARD|BARCLAYS|BAREFOOT|BARGAINS|BASEBALL|BASKETBALL|BAUHAUS|BAYERN|BB|BBC|BBT|BBVA|BCG|BCN|BD|BE|BEATS|BEAUTY|BEER|BENTLEY|BERLIN|BEST|BESTBUY|BET|BF|BG|BH|BHARTI|BI|BIBLE|BID|BIKE|BING|BINGO|BIO|BIZ|BJ|BLACK|BLACKFRIDAY|BLOCKBUSTER|BLOG|BLOOMBERG|BLUE|BM|BMS|BMW|BN|BNPPARIBAS|BO|BOATS|BOEHRINGER|BOFA|BOM|BOND|BOO|BOOK|BOOKING|BOSCH|BOSTIK|BOSTON|BOT|BOUTIQUE|BOX|BR|BRADESCO|BRIDGESTONE|BROADWAY|BROKER|BROTHER|BRUSSELS|BS|BT|BUDAPEST|BUGATTI|BUILD|BUILDERS|BUSINESS|BUY|BUZZ|BV|BW|BY|BZ|BZH|CA|CAB|CAFE|CAL|CALL|CALVINKLEIN|CAM|CAMERA|CAMP|CANCERRESEARCH|CANON|CAPETOWN|CAPITAL|CAPITALONE|CAR|CARAVAN|CARDS|CARE|CAREER|CAREERS|CARS|CASA|CASE|CASEIH|CASH|CASINO|CAT|CATERING|CATHOLIC|CBA|CBN|CBRE|CBS|CC|CD|CEB|CENTER|CEO|CERN|CF|CFA|CFD|CG|CH|CHANEL|CHANNEL|CHARITY|CHASE|CHAT|CHEAP|CHINTAI|CHRISTMAS|CHROME|CHURCH|CI|CIPRIANI|CIRCLE|CISCO|CITADEL|CITI|CITIC|CITY|CITYEATS|CK|CL|CLAIMS|CLEANING|CLICK|CLINIC|CLINIQUE|CLOTHING|CLOUD|CLUB|CLUBMED|CM|CN|CO|COACH|CODES|COFFEE|COLLEGE|COLOGNE|COM|COMCAST|COMMBANK|COMMUNITY|COMPANY|COMPARE|COMPUTER|COMSEC|CONDOS|CONSTRUCTION|CONSULTING|CONTACT|CONTRACTORS|COOKING|COOKINGCHANNEL|COOL|COOP|CORSICA|COUNTRY|COUPON|COUPONS|COURSES|CPA|CR|CREDIT|CREDITCARD|CREDITUNION|CRICKET|CROWN|CRS|CRUISE|CRUISES|CSC|CU|CUISINELLA|CV|CW|CX|CY|CYMRU|CYOU|CZ|DABUR|DAD|DANCE|DATA|DATE|DATING|DATSUN|DAY|DCLK|DDS|DE|DEAL|DEALER|DEALS|DEGREE|DELIVERY|DELL|DELOITTE|DELTA|DEMOCRAT|DENTAL|DENTIST|DESI|DESIGN|DEV|DHL|DIAMONDS|DIET|DIGITAL|DIRECT|DIRECTORY|DISCOUNT|DISCOVER|DISH|DIY|DJ|DK|DM|DNP|DO|DOCS|DOCTOR|DOG|DOMAINS|DOT|DOWNLOAD|DRIVE|DTV|DUBAI|DUCK|DUNLOP|DUPONT|DURBAN|DVAG|DVR|DZ|EARTH|EAT|EC|ECO|EDEKA|EDU|EDUCATION|EE|EG|EMAIL|EMERCK|ENERGY|ENGINEER|ENGINEERING|ENTERPRISES|EPSON|EQUIPMENT|ER|ERICSSON|ERNI|ES|ESQ|ESTATE|ET|ETISALAT|EU|EUROVISION|EUS|EVENTS|EXCHANGE|EXPERT|EXPOSED|EXPRESS|EXTRASPACE|FAGE|FAIL|FAIRWINDS|FAITH|FAMILY|FAN|FANS|FARM|FARMERS|FASHION|FAST|FEDEX|FEEDBACK|FERRARI|FERRERO|FI|FIAT|FIDELITY|FIDO|FILM|FINAL|FINANCE|FINANCIAL|FIRE|FIRESTONE|FIRMDALE|FISH|FISHING|FIT|FITNESS|FJ|FK|FLICKR|FLIGHTS|FLIR|FLORIST|FLOWERS|FLY|FM|FO|FOO|FOOD|FOODNETWORK|FOOTBALL|FORD|FOREX|FORSALE|FORUM|FOUNDATION|FOX|FR|FREE|FRESENIUS|FRL|FROGANS|FRONTDOOR|FRONTIER|FTR|FUJITSU|FUJIXEROX|FUN|FUND|FURNITURE|FUTBOL|FYI|GA|GAL|GALLERY|GALLO|GALLUP|GAME|GAMES|GAP|GARDEN|GAY|GB|GBIZ|GD|GDN|GE|GEA|GENT|GENTING|GEORGE|GF|GG|GGEE|GH|GI|GIFT|GIFTS|GIVES|GIVING|GL|GLADE|GLASS|GLE|GLOBAL|GLOBO|GM|GMAIL|GMBH|GMO|GMX|GN|GODADDY|GOLD|GOLDPOINT|GOLF|GOO|GOODYEAR|GOOG|GOOGLE|GOP|GOT|GOV|GP|GQ|GR|GRAINGER|GRAPHICS|GRATIS|GREEN|GRIPE|GROCERY|GROUP|GS|GT|GU|GUARDIAN|GUCCI|GUGE|GUIDE|GUITARS|GURU|GW|GY|HAIR|HAMBURG|HANGOUT|HAUS|HBO|HDFC|HDFCBANK|HEALTH|HEALTHCARE|HELP|HELSINKI|HERE|HERMES|HGTV|HIPHOP|HISAMITSU|HITACHI|HIV|HK|HKT|HM|HN|HOCKEY|HOLDINGS|HOLIDAY|HOMEDEPOT|HOMEGOODS|HOMES|HOMESENSE|HONDA|HORSE|HOSPITAL|HOST|HOSTING|HOT|HOTELES|HOTELS|HOTMAIL|HOUSE|HOW|HR|HSBC|HT|HU|HUGHES|HYATT|HYUNDAI|IBM|ICBC|ICE|ICU|ID|IE|IEEE|IFM|IKANO|IL|IM|IMAMAT|IMDB|IMMO|IMMOBILIEN|IN|INC|INDUSTRIES|INFINITI|INFO|ING|INK|INSTITUTE|INSURANCE|INSURE|INT|INTEL|INTERNATIONAL|INTUIT|INVESTMENTS|IO|IPIRANGA|IQ|IR|IRISH|IS|ISMAILI|IST|ISTANBUL|IT|ITAU|ITV|IVECO|JAGUAR|JAVA|JCB|JCP|JE|JEEP|JETZT|JEWELRY|JIO|JLL|JM|JMP|JNJ|JO|JOBS|JOBURG|JOT|JOY|JP|JPMORGAN|JPRS|JUEGOS|JUNIPER|KAUFEN|KDDI|KE|KERRYHOTELS|KERRYLOGISTICS|KERRYPROPERTIES|KFH|KG|KH|KI|KIA|KIM|KINDER|KINDLE|KITCHEN|KIWI|KM|KN|KOELN|KOMATSU|KOSHER|KP|KPMG|KPN|KR|KRD|KRED|KUOKGROUP|KW|KY|KYOTO|KZ|LA|LACAIXA|LAMBORGHINI|LAMER|LANCASTER|LANCIA|LAND|LANDROVER|LANXESS|LASALLE|LAT|LATINO|LATROBE|LAW|LAWYER|LB|LC|LDS|LEASE|LECLERC|LEFRAK|LEGAL|LEGO|LEXUS|LGBT|LI|LIDL|LIFE|LIFEINSURANCE|LIFESTYLE|LIGHTING|LIKE|LILLY|LIMITED|LIMO|LINCOLN|LINDE|LINK|LIPSY|LIVE|LIVING|LIXIL|LK|LLC|LLP|LOAN|LOANS|LOCKER|LOCUS|LOFT|LOL|LONDON|LOTTE|LOTTO|LOVE|LPL|LPLFINANCIAL|LR|LS|LT|LTD|LTDA|LU|LUNDBECK|LUPIN|LUXE|LUXURY|LV|LY|MA|MACYS|MADRID|MAIF|MAISON|MAKEUP|MAN|MANAGEMENT|MANGO|MAP|MARKET|MARKETING|MARKETS|MARRIOTT|MARSHALLS|MASERATI|MATTEL|MBA|MC|MCKINSEY|MD|ME|MED|MEDIA|MEET|MELBOURNE|MEME|MEMORIAL|MEN|MENU|MERCKMSD|METLIFE|MG|MH|MIAMI|MICROSOFT|MIL|MINI|MINT|MIT|MITSUBISHI|MK|ML|MLB|MLS|MM|MMA|MN|MO|MOBI|MOBILE|MODA|MOE|MOI|MOM|MONASH|MONEY|MONSTER|MORMON|MORTGAGE|MOSCOW|MOTO|MOTORCYCLES|MOV|MOVIE|MP|MQ|MR|MS|MSD|MT|MTN|MTR|MU|MUSEUM|MUTUAL|MV|MW|MX|MY|MZ|NA|NAB|NAGOYA|NAME|NATIONWIDE|NATURA|NAVY|NBA|NC|NE|NEC|NET|NETBANK|NETFLIX|NETWORK|NEUSTAR|NEW|NEWHOLLAND|NEWS|NEXT|NEXTDIRECT|NEXUS|NF|NFL|NG|NGO|NHK|NI|NICO|NIKE|NIKON|NINJA|NISSAN|NISSAY|NL|NO|NOKIA|NORTHWESTERNMUTUAL|NORTON|NOW|NOWRUZ|NOWTV|NP|NR|NRA|NRW|NTT|NU|NYC|NZ|OBI|OBSERVER|OFF|OFFICE|OKINAWA|OLAYAN|OLAYANGROUP|OLDNAVY|OLLO|OM|OMEGA|ONE|ONG|ONL|ONLINE|ONYOURSIDE|OOO|OPEN|ORACLE|ORANGE|ORG|ORGANIC|ORIGINS|OSAKA|OTSUKA|OTT|OVH|PA|PAGE|PANASONIC|PARIS|PARS|PARTNERS|PARTS|PARTY|PASSAGENS|PAY|PCCW|PE|PET|PF|PFIZER|PG|PH|PHARMACY|PHD|PHILIPS|PHONE|PHOTO|PHOTOGRAPHY|PHOTOS|PHYSIO|PICS|PICTET|PICTURES|PID|PIN|PING|PINK|PIONEER|PIZZA|PK|PL|PLACE|PLAY|PLAYSTATION|PLUMBING|PLUS|PM|PN|PNC|POHL|POKER|POLITIE|PORN|POST|PR|PRAMERICA|PRAXI|PRESS|PRIME|PRO|PROD|PRODUCTIONS|PROF|PROGRESSIVE|PROMO|PROPERTIES|PROPERTY|PROTECTION|PRU|PRUDENTIAL|PS|PT|PUB|PW|PWC|PY|QA|QPON|QUEBEC|QUEST|QVC|RACING|RADIO|RAID|RE|READ|REALESTATE|REALTOR|REALTY|RECIPES|RED|REDSTONE|REDUMBRELLA|REHAB|REISE|REISEN|REIT|RELIANCE|REN|RENT|RENTALS|REPAIR|REPORT|REPUBLICAN|REST|RESTAURANT|REVIEW|REVIEWS|REXROTH|RICH|RICHARDLI|RICOH|RIL|RIO|RIP|RMIT|RO|ROCHER|ROCKS|RODEO|ROGERS|ROOM|RS|RSVP|RU|RUGBY|RUHR|RUN|RW|RWE|RYUKYU|SA|SAARLAND|SAFE|SAFETY|SAKURA|SALE|SALON|SAMSCLUB|SAMSUNG|SANDVIK|SANDVIKCOROMANT|SANOFI|SAP|SARL|SAS|SAVE|SAXO|SB|SBI|SBS|SC|SCA|SCB|SCHAEFFLER|SCHMIDT|SCHOLARSHIPS|SCHOOL|SCHULE|SCHWARZ|SCIENCE|SCJOHNSON|SCOT|SD|SE|SEARCH|SEAT|SECURE|SECURITY|SEEK|SELECT|SENER|SERVICES|SES|SEVEN|SEW|SEX|SEXY|SFR|SG|SH|SHANGRILA|SHARP|SHAW|SHELL|SHIA|SHIKSHA|SHOES|SHOP|SHOPPING|SHOUJI|SHOW|SHOWTIME|SHRIRAM|SI|SILK|SINA|SINGLES|SITE|SJ|SK|SKI|SKIN|SKY|SKYPE|SL|SLING|SM|SMART|SMILE|SN|SNCF|SO|SOCCER|SOCIAL|SOFTBANK|SOFTWARE|SOHU|SOLAR|SOLUTIONS|SONG|SONY|SOY|SPACE|SPORT|SPOT|SPREADBETTING|SR|SRL|SS|ST|STADA|STAPLES|STAR|STATEBANK|STATEFARM|STC|STCGROUP|STOCKHOLM|STORAGE|STORE|STREAM|STUDIO|STUDY|STYLE|SU|SUCKS|SUPPLIES|SUPPLY|SUPPORT|SURF|SURGERY|SUZUKI|SV|SWATCH|SWIFTCOVER|SWISS|SX|SY|SYDNEY|SYSTEMS|SZ|TAB|TAIPEI|TALK|TAOBAO|TARGET|TATAMOTORS|TATAR|TATTOO|TAX|TAXI|TC|TCI|TD|TDK|TEAM|TECH|TECHNOLOGY|TEL|TEMASEK|TENNIS|TEVA|TF|TG|TH|THD|THEATER|THEATRE|TIAA|TICKETS|TIENDA|TIFFANY|TIPS|TIRES|TIROL|TJ|TJMAXX|TJX|TK|TKMAXX|TL|TM|TMALL|TN|TO|TODAY|TOKYO|TOOLS|TOP|TORAY|TOSHIBA|TOTAL|TOURS|TOWN|TOYOTA|TOYS|TR|TRADE|TRADING|TRAINING|TRAVEL|TRAVELCHANNEL|TRAVELERS|TRAVELERSINSURANCE|TRUST|TRV|TT|TUBE|TUI|TUNES|TUSHU|TV|TVS|TW|TZ|UA|UBANK|UBS|UG|UK|UNICOM|UNIVERSITY|UNO|UOL|UPS|US|UY|UZ|VA|VACATIONS|VANA|VANGUARD|VC|VE|VEGAS|VENTURES|VERISIGN|VERSICHERUNG|VET|VG|VI|VIAJES|VIDEO|VIG|VIKING|VILLAS|VIN|VIP|VIRGIN|VISA|VISION|VIVA|VIVO|VLAANDEREN|VN|VODKA|VOLKSWAGEN|VOLVO|VOTE|VOTING|VOTO|VOYAGE|VU|VUELOS|WALES|WALMART|WALTER|WANG|WANGGOU|WATCH|WATCHES|WEATHER|WEATHERCHANNEL|WEBCAM|WEBER|WEBSITE|WED|WEDDING|WEIBO|WEIR|WF|WHOSWHO|WIEN|WIKI|WILLIAMHILL|WIN|WINDOWS|WINE|WINNERS|WME|WOLTERSKLUWER|WOODSIDE|WORK|WORKS|WORLD|WOW|WS|WTC|WTF|XBOX|XEROX|XFINITY|XIHUAN|XIN|XN--11B4C3D|XN--1CK2E1B|XN--1QQW23A|XN--2SCRJ9C|XN--30RR7Y|XN--3BST00M|XN--3DS443G|XN--3E0B707E|XN--3HCRJ9C|XN--3OQ18VL8PN36A|XN--3PXU8K|XN--42C2D9A|XN--45BR5CYL|XN--45BRJ9C|XN--45Q11C|XN--4GBRIM|XN--54B7FTA0CC|XN--55QW42G|XN--55QX5D|XN--5SU34J936BGSG|XN--5TZM5G|XN--6FRZ82G|XN--6QQ986B3XL|XN--80ADXHKS|XN--80AO21A|XN--80AQECDR1A|XN--80ASEHDB|XN--80ASWG|XN--8Y0A063A|XN--90A3AC|XN--90AE|XN--90AIS|XN--9DBQ2A|XN--9ET52U|XN--9KRT00A|XN--B4W605FERD|XN--BCK1B9A5DRE4C|XN--C1AVG|XN--C2BR7G|XN--CCK2B3B|XN--CCKWCXETD|XN--CG4BKI|XN--CLCHC0EA0B2G2A9GCD|XN--CZR694B|XN--CZRS0T|XN--CZRU2D|XN--D1ACJ3B|XN--D1ALF|XN--E1A4C|XN--ECKVDTC9D|XN--EFVY88H|XN--FCT429K|XN--FHBEI|XN--FIQ228C5HS|XN--FIQ64B|XN--FIQS8S|XN--FIQZ9S|XN--FJQ720A|XN--FLW351E|XN--FPCRJ9C3D|XN--FZC2C9E2C|XN--FZYS8D69UVGM|XN--G2XX48C|XN--GCKR3F0F|XN--GECRJ9C|XN--GK3AT1E|XN--H2BREG3EVE|XN--H2BRJ9C|XN--H2BRJ9C8C|XN--HXT814E|XN--I1B6B1A6A2E|XN--IMR513N|XN--IO0A7I|XN--J1AEF|XN--J1AMH|XN--J6W193G|XN--JLQ480N2RG|XN--JLQ61U9W7B|XN--JVR189M|XN--KCRX77D1X4A|XN--KPRW13D|XN--KPRY57D|XN--KPUT3I|XN--L1ACC|XN--LGBBAT1AD8J|XN--MGB9AWBF|XN--MGBA3A3EJT|XN--MGBA3A4F16A|XN--MGBA7C0BBN0A|XN--MGBAAKC7DVF|XN--MGBAAM7A8H|XN--MGBAB2BD|XN--MGBAH1A3HJKRD|XN--MGBAI9AZGQP6J|XN--MGBAYH7GPA|XN--MGBBH1A|XN--MGBBH1A71E|XN--MGBC0A9AZCG|XN--MGBCA7DZDO|XN--MGBCPQ6GPA1A|XN--MGBERP4A5D4AR|XN--MGBGU82A|XN--MGBI4ECEXP|XN--MGBPL2FH|XN--MGBT3DHD|XN--MGBTX2B|XN--MGBX4CD0AB|XN--MIX891F|XN--MK1BU44C|XN--MXTQ1M|XN--NGBC5AZD|XN--NGBE9E0A|XN--NGBRX|XN--NODE|XN--NQV7F|XN--NQV7FS00EMA|XN--NYQY26A|XN--O3CW4H|XN--OGBPF8FL|XN--OTU796D|XN--P1ACF|XN--P1AI|XN--PGBS0DH|XN--PSSY2U|XN--Q7CE6A|XN--Q9JYB4C|XN--QCKA1PMC|XN--QXA6A|XN--QXAM|XN--RHQV96G|XN--ROVU88B|XN--RVC1E0AM3E|XN--S9BRJ9C|XN--SES554G|XN--T60B56A|XN--TCKWE|XN--TIQ49XQYJ|XN--UNUP4Y|XN--VERMGENSBERATER-CTB|XN--VERMGENSBERATUNG-PWB|XN--VHQUV|XN--VUQ861B|XN--W4R85EL8FHU5DNRA|XN--W4RS40L|XN--WGBH1C|XN--WGBL6A|XN--XHQ521B|XN--XKC2AL3HYE2A|XN--XKC2DL3A5EE0H|XN--Y9A3AQ|XN--YFRO4I67O|XN--YGBI2AMMX|XN--ZFR164B|XXX|XYZ|YACHTS|YAHOO|YAMAXUN|YANDEX|YE|YODOBASHI|YOGA|YOKOHAMA|YOU|YOUTUBE|YT|YUN|ZA|ZAPPOS|ZARA|ZERO|ZIP|ZM|ZONE|ZUERICH|ZW)";
                Regex loRegexIsInternetEmailAddressAtTLD = new Regex("^" + lsEmailFormat + "@" + lsTLDList + @"\b$", RegexOptions.IgnoreCase);

                if (!loRegexIsInternetEmailAddressAtTLD.IsMatch(lsEmail))
                {
                    string lsServerFormat = @"(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)";
                    Regex loRegexIsInternetEmailAddress = new Regex("^" + lsEmailFormat + "@" + lsServerFormat + "+" + lsTLDList + @"\b$", RegexOptions.IgnoreCase);
                    if (!loRegexIsInternetEmailAddress.IsMatch(lsEmail))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
