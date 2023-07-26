using UnityEngine;
using TMPro;

public class MessageList : MonoBehaviour
{
    public GameObject chatMessageTemplate;
    public TMP_InputField typeMessageInput;
    public string targetUsername;

    public Message[] messageHistory;
    private int messageIndex;

    private void Start()
    {
        messageHistory = new Message[50];
        messageIndex = 0;
    }

    public void CreateMessage(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            if (messageIndex >= 50)
            {
                messageIndex = 0;
            }
            if (ContainsSwearWords(content))
            {
                content = CensorSwearWords(content);
            }
            if (chatMessageTemplate != null)
            {
                // Create a new message object as a child of the current object
                GameObject newMessageObject = Instantiate(chatMessageTemplate, transform);

                // Reset the local position of the new message object
                RectTransform newMessageTransform = newMessageObject.GetComponent<RectTransform>();
                newMessageTransform.localPosition = Vector3.zero;

                Message messageComponent = newMessageObject.GetComponent<Message>();

                if (messageComponent != null)
                {
                    string currentDate = System.DateTime.Now.ToString();
                    messageComponent.SetContent(currentDate, targetUsername, content);
                    messageHistory[messageIndex] = messageComponent;
                    messageIndex++;
                }
                else
                {
                    Debug.LogWarning("ChatMessageTemplate does not have the Message component attached.");
                }
            }
            else
            {
                Debug.LogWarning("ChatMessageTemplate is not assigned.");
            }

            typeMessageInput.text = string.Empty;

            // Print the latest message
            Message latestMessage = messageHistory[messageIndex - 1];
            Debug.Log("Content: " + latestMessage.messageText);
        }
    }
    private bool ContainsSwearWords(string message)
    {
        foreach (string swearWord in swearWords)
        {
            if (message.ToLower().Contains(swearWord.ToLower()))
            {
                return true;
            }
        }
        return false;
    }
    private string CensorSwearWords(string message)
    {
        foreach (string swearWord in swearWords)
        {
            string censorString = new string('*', swearWord.Length);
            message = message.Replace(swearWord, censorString);
        }
        return message;
    }
    string[] swearWords = new string[]
    {
        "anal",
        "analannie",
        "analsex",
        "anus",
        "arse",
        "arsehole",
        "ass",
        "assbagger",
        "assblaster",
        "assclown",
        "asscowboy",
        "asses",
        "assfuck",
        "assfucker",
        "asshat",
        "asshole",
        "assholes",
        "asshore",
        "assjockey",
        "asskiss",
        "asskisser",
        "assklown",
        "asslick",
        "asslicker",
        "asslover",
        "assman",
        "assmonkey",
        "assmunch",
        "assmuncher",
        "asspacker",
        "asspirate",
        "asspuppies",
        "assranger",
        "asswhore",
        "asswipe",
        "backdoorman",
        "badfuck",
        "balllicker",
        "ballsack",
        "barelylegal",
        "barface",
        "barfface",
        "bastard" ,
        "beastality",
        "beastial",
        "beastiality",
        "beatoff",
        "beat-off",
        "beatyourmeat",
        "bestiality",
        "biatch",
        "bigass",
        "bigbastard",
        "bigbutt",
        "bitch",
        "bitcher",
        "bitches",
        "bitchez",
        "bitchin",
        "bitching",
        "bitchslap",
        "bitchy",
        "biteme",
        "blowjob",
        "bohunk",
        "bollick",
        "bollock",
        "bondage",
        "boner",
        "boob",
        "boobies",
        "boobs",
        "booby",
        "boody",
        "boong",
        "boonga",
        "boonie",
        "booty",
        "bootycall",
        "bountybar",
        "breastjob",
        "breastlover",
        "breastman",
        "brothel",
        "bugger",
        "buggered",
        "buggery",
        "bullcrap",
        "bulldike",
        "bulldyke",
        "bullshit",
        "bumblefuck",
        "bumfuck",
        "bunga",
        "bunghole",
        "butchbabes",
        "butchdike",
        "butchdyke",
        "butt",
        "buttbang",
        "butt-bang",
        "buttface",
        "buttfuck",
        "butt-fuck",
        "buttfucker",
        "butt-fucker",
        "buttfuckers",
        "butt-fuckers",
        "butthead",
        "buttman",
        "buttmunch",
        "buttmuncher",
        "buttpirate",
        "buttplug",
        "buttstain",
        "byatch",
        "cacker",
        "cameljockey",
        "cameltoe",
        "carpetmuncher",
        "cherrypopper",
        "chickslick",
        "clamdigger",
        "clamdiver",
        "clit",
        "clitoris",
        "clogwog",
        "cocaine",
        "cock",
        "cockblock",
        "cockblocker",
        "cockcowboy",
        "cockfight",
        "cockhead",
        "cockknob",
        "cocklicker",
        "cocklover",
        "cocknob",
        "cockqueen",
        "cockrider",
        "cocksman",
        "cocksmith",
        "cocksmoker",
        "cocksucer",
        "cocksuck" ,
        "cocksucked" ,
        "cocksucker",
        "cocksucking",
        "cocktease",
        "coitus",
        "condom",
        "copulate",
        "cornhole",
        "crackpipe",
        "crackwhore",
        "crack-whore",
        "crapola",
        "crapper",
        "crotchjockey",
        "crotchmonkey",
        "crotchrot",
        "cum",
        "cumbubble",
        "cumfest",
        "cumjockey",
        "cumm",
        "cummer",
        "cumming",
        "cumquat",
        "cumqueen",
        "cumshot",
        "cunilingus",
        "cunillingus",
        "cunn",
        "cunnilingus",
        "cunntt",
        "cunt",
        "cunteyed",
        "cuntfuck",
        "cuntfucker",
        "cuntlick" ,
        "cuntlicker" ,
        "cuntlicking" ,
        "cuntsucker",
        "cybersex",
        "cyberslimer",
        "darkie",
        "darky",
        "datnigga",
        "deapthroat",
        "deepthroat",
        "dick",
        "dickbrain",
        "dickforbrains",
        "dickhead",
        "dickless",
        "dicklick",
        "dicklicker",
        "dickman",
        "dickwad",
        "dickweed",
        "diddle",
        "dike",
        "dildo",
        "dingleberry",
        "dipshit",
        "dipstick",
        "dixiedike",
        "dixiedyke",
        "doggiestyle",
        "doggystyle",
        "dragqueen",
        "dragqween",
        "dripdick",
        "dumb",
        "dumbass",
        "dumbbitch",
        "dumbfuck",
        "dyefly",
        "dyke",
        "easyslut",
        "eatballs",
        "eatme",
        "eatpussy",
        "ejaculate",
        "ejaculated",
        "ejaculating" ,
        "ejaculation",
        "erection",
        "facefucker",
        "faeces",
        "fag",
        "fagging",
        "faggot",
        "fagot",
        "fannyfucker",
        "fastfuck",
        "fatah",
        "fatass",
        "fatfuck",
        "fatfucker",
        "fatso",
        "fckcum",
        "feces",
        "fingerfood",
        "fingerfuck" ,
        "fingerfucked" ,
        "fingerfucker" ,
        "fingerfuckers",
        "fingerfucking" ,
        "fistfuck",
        "fistfucked" ,
        "fistfucker" ,
        "fistfucking" ,
        "fisting",
        "flange",
        "flasher",
        "flatulence",
        "floo",
        "flydie",
        "flydye",
        "fondle",
        "footaction",
        "footfuck",
        "footfucker",
        "footlicker",
        "footstar",
        "fore",
        "foreskin",
        "forni",
        "fornicate",
        "foursome",
        "freakfuck",
        "freakyfucker",
        "freefuck",
        "fucck",
        "fuck",
        "fucka",
        "fuckable",
        "fuckbag",
        "fuckbuddy",
        "fucked",
        "fuckedup",
        "fucker",
        "fuckers",
        "fuckface",
        "fuckfest",
        "fuckfreak",
        "fuckfriend",
        "fuckhead",
        "fuckher",
        "fuckin",
        "fuckina",
        "fucking",
        "fuckingbitch",
        "fuckinnuts",
        "fuckinright",
        "fuckit",
        "fuckknob",
        "fuckme" ,
        "fuckmehard",
        "fuckmonkey",
        "fuckoff",
        "fuckpig",
        "fucks",
        "fucktard",
        "fuckwhore",
        "fuckyou",
        "fudgepacker",
        "fugly",
        "funfuck",
        "fuuck",
        "gangbang",
        "gangbanged" ,
        "gangbanger",
        "gatorbait",
        "gaymuthafuckinwhore",
        "gaysex" ,
        "genital",
        "getiton",
        "givehead",
        "glazeddonut",
        "godammit",
        "goddamit",
        "goddammit",
        "goddamn",
        "goddamned",
        "goddamnes",
        "goddamnit",
        "goddamnmuthafucker",
        "goldenshower",
        "gonorrehea",
        "gotohell",
        "goy",
        "goyim",
        "gringo",
        "grostulation",
        "gyp",
        "gypo",
        "gypp",
        "gyppie",
        "gyppo",
        "gyppy",
        "handjob",
        "hardon",
        "headfuck",
        "heeb",
        "henhouse",
        "heroin",
        "hindoo",
        "hitler",
        "hitlerism",
        "hitlerist",
        "hoes",
        "holestuffer",
        "homo",
        "homobangers",
        "hooker",
        "hookers",
        "hooters",
        "horny",
        "horseshit",
        "hotdamn",
        "hotpussy",
        "hottotrot",
        "husky",
        "hussy",
        "hustler",
        "hymen",
        "hymie",
        "iblowu",
        "idiot",
        "incest",
        "insest",
        "intercourse",
        "interracial",
        "intheass",
        "inthebuff",
        "jackass",
        "jackoff",
        "jackshit",
        "jerkoff",
        "jiga",
        "jigaboo",
        "jigg",
        "jigga",
        "jiggabo",
        "jigger" ,
        "jiggy",
        "jihad",
        "jijjiboo",
        "jimfish",
        "jism",
        "jiz" ,
        "jizim",
        "jizjuice",
        "jizm" ,
        "jizz",
        "jizzim",
        "jizzum",
        "joint",
        "juggalo",
        "jugs",
        "junglebunny",
        "kigger",
        "kissass",
        "kkk",
        "kotex",
        "kraut",
        "kum",
        "kumbubble",
        "kumbullbe",
        "kummer",
        "kumming",
        "kumquat",
        "kums",
        "kunilingus",
        "kunnilingus",
        "kunt",
        "lactate",
        "laid",
        "lapdance",
        "lesbain",
        "lesbayn",
        "lesbian",
        "lesbin",
        "lesbo",
        "lezbe",
        "lezbefriends",
        "lezbo",
        "licker",
        "lickme",
        "limpdick",
        "livesex",
        "looser",
        "loser",
        "lovebone",
        "lovejuice",
        "lsd",
        "lubejob",
        "luckycammeltoe",
        "macaca",
        "mams",
        "manhater",
        "manpaste",
        "marijuana",
        "mastabate",
        "mastabater",
        "masterbate",
        "masterblaster",
        "mastrabator",
        "masturbate",
        "masturbating",
        "mattressprincess",
        "meatbeatter",
        "meatrack",
        "meth",
        "mgger",
        "mggor",
        "mickeyfinn",
        "mideast",
        "milf",
        "mofo",
        "moky",
        "mooncricket",
        "moron",
        "moslem",
        "mosshead",
        "mothafuck",
        "mothafucka",
        "mothafuckaz",
        "mothafucked" ,
        "mothafucker",
        "mothafuckin",
        "mothafucking" ,
        "mothafuckings",
        "motherfuck",
        "motherfucked",
        "motherfucker",
        "motherfuckin",
        "motherfucking",
        "motherfuckings",
        "motherlovebone",
        "muff",
        "muffdive",
        "muffdiver",
        "muffindiver",
        "mufflikcer",
        "mulatto",
        "muncher",
        "naked",
        "narcotic",
        "nastybitch",
        "nastyho",
        "nastyslut",
        "nastywhore",
        "nazi",
        "negro",
        "negroes",
        "negroid",
        "negro's",
        "nigg",
        "nigga",
        "niggah",
        "niggaracci",
        "niggard",
        "niggarded",
        "niggarding",
        "niggardliness",
        "niggardliness's",
        "niggardly",
        "niggards",
        "niggard's",
        "niggaz",
        "nigger",
        "niggerhead",
        "niggerhole",
        "niggers",
        "nigger's",
        "niggle",
        "niggled",
        "niggles",
        "niggling",
        "nigglings",
        "niggor",
        "niggur",
        "niglet",
        "nignog",
        "nigr",
        "nigra",
        "nigre",
        "nipple",
        "nipplering",
        "nittit",
        "nlgger",
        "nlggor",
        "nofuckingway",
        "nook",
        "nookey",
        "nookie",
        "nude",
        "nudger",
        "nutfucker",
        "nymph",
        "ontherag",
        "oral",
        "orga",
        "orgasim" ,
        "orgasm",
        "orgies",
        "orgy",
        "osama",
        "paki",
        "palesimian",
        "payo",
        "pearlnecklace",
        "pecker",
        "peckerwood",
        "peehole",
        "pee-pee",
        "peepshow",
        "peepshpw",
        "peni5",
        "penile",
        "penis",
        "penises",
        "penthouse",
        "phonesex",
        "phuk",
        "phuked",
        "phuking",
        "phukked",
        "phukking",
        "phungky",
        "phuq",
        "pi55",
        "piker",
        "pimp",
        "pimped",
        "pimper",
        "pimpjuic",
        "pimpjuice",
        "pimpsimp",
        "pindick",
        "pisser",
        "pisses" ,
        "pisshead",
        "pissin" ,
        "pissing",
        "pissoff" ,
        "playboy",
        "playgirl",
        "pocha",
        "pocho",
        "pom",
        "pommie",
        "pommy",
        "poo",
        "poon",
        "poontang",
        "poop",
        "pooper",
        "pooperscooper",
        "pooping",
        "poorwhitetrash",
        "popimp",
        "porchmonkey",
        "porn",
        "pornflick",
        "pornking",
        "porno",
        "pornography",
        "pornprincess",
        "pric",
        "prick",
        "prickhead",
        "prostitute",
        "pu55i",
        "pu55y",
        "pubiclice",
        "pud",
        "pudboy",
        "pudd",
        "puddboy",
        "puntang",
        "purinapricness",
        "puss",
        "pussie",
        "pussies",
        "pussy",
        "pussycat",
        "pussyeater",
        "pussyfucker",
        "pussylicker",
        "pussylips",
        "pussylover",
        "pussypounder",
        "pusy",
        "quashie",
        "queef",
        "queer",
        "quim",
        "ra8s",
        "raghead",
        "rape",
        "raped",
        "raper",
        "rapist",
        "rectum",
        "reefer",
        "reestie",
        "rentafuck",
        "rere",
        "retard",
        "retarded",
        "ribbed",
        "rigger",
        "rimjob",
        "rimming",
        "roundeye",
        "rump",
        "russki",
        "russkie",
        "sadis",
        "sadom",
        "samckdaddy",
        "sandm",
        "sandnigger",
        "satan",
        "scag",
        "scallywag",
        "scat",
        "schlong",
        "screw",
        "screwyou",
        "scrotum",
        "scum",
        "semen",
        "seppo",
        "sex",
        "sexed",
        "sexfarm",
        "sexhound",
        "sexhouse",
        "sexing",
        "sexkitten",
        "sexpot",
        "sexslave",
        "sextogo",
        "sextoy",
        "sextoys",
        "sexual",
        "sexually",
        "sexwhore",
        "sexymoma",
        "sexy-slim",
        "shag",
        "shaggin",
        "shagging",
        "shat",
        "shav",
        "shawtypimp",
        "sheeney",
        "shhit",
        "shinola",
        "shit",
        "shitcan",
        "shitdick",
        "shite",
        "shiteater",
        "shited",
        "shitface",
        "shitfaced",
        "shitfit",
        "shitforbrains",
        "shitfuck",
        "shitfucker",
        "shitfull",
        "shithapens",
        "shithappens",
        "shithead",
        "shithouse",
        "shiting",
        "shitlist",
        "shitola",
        "shitoutofluck",
        "shits",
        "shitstain",
        "shitted",
        "shitter",
        "shitting",
        "shitty" ,
        "shortfuck",
        "sissy",
        "sixsixsix",
        "sixtynine",
        "sixtyniner",
        "skank",
        "skankbitch",
        "skankfuck",
        "skankwhore",
        "skanky",
        "skankybitch",
        "skankywhore",
        "skinflute",
        "skum",
        "skumbag",
        "slant",
        "slanteye",
        "slapper",
        "slaughter",
        "slavedriver",
        "sleezebag",
        "sleezeball",
        "slideitin",
        "slimeball",
        "slimebucket",
        "slopehead",
        "slopey",
        "slopy",
        "slut",
        "sluts",
        "slutt",
        "slutting",
        "slutty",
        "slutwear",
        "slutwhore",
        "smackthemonkey",
        "smut",
        "snatch",
        "snatchpatch",
        "snigger",
        "sniggered",
        "sniggering",
        "sniggers",
        "snigger's",
        "snot",
        "snownigger",
        "sodomite",
        "sonofabitch",
        "sonofbitch",
        "sooty",
        "spaghettibender",
        "spaghettinigger",
        "spank",
        "spankthemonkey",
        "sperm",
        "spermacide",
        "spermbag",
        "spermhearder",
        "spermherder",
        "spic",
        "spick",
        "spig",
        "spigotty",
        "spik",
        "splittail",
        "spooge",
        "spreadeagle",
        "spunk",
        "spunky",
        "stiffy",
        "strapon",
        "stringer",
        "stripclub",
        "stroking",
        "stupid",
        "stupidfuck",
        "stupidfucker",
        "suck",
        "suckdick",
        "sucker",
        "suckme",
        "suckmyass",
        "suckmydick",
        "suckmytit",
        "suckoff",
        "swallower",
        "swastika",
        "syphilis",
        "tang",
        "tarbaby",
        "terrorist",
        "teste",
        "testicle",
        "testicles",
        "thicklips",
        "threesome",
        "timbernigger",
        "tit",
        "titbitnipply",
        "titfuck",
        "titfucker",
        "titfuckin",
        "titjob",
        "titlicker",
        "titlover",
        "tits",
        "tittie",
        "titties",
        "titty",
        "tongethruster",
        "tonguethrust",
        "tonguetramp",
        "tosser",
        "towelhead",
        "trailertrash",
        "trannie",
        "tranny",
        "transexual",
        "transsexual",
        "transvestite",
        "trisexual",
        "trojan",
        "trots",
        "tuckahoe",
        "tunneloflove",
        "turd",
        "twat",
        "twink",
        "twinkie",
        "twobitwhore",
        "unfuckable",
        "uptheass",
        "upthebutt",
        "usama",
        "uterus",
        "vagina",
        "vaginal",
        "vietcong",
        "virgin",
        "virginbreaker",
        "vulva",
        "wab",
        "wank",
        "wanker",
        "wanking",
        "waysted",
        "weapon",
        "weenie",
        "weewee",
        "welcher",
        "welfare",
        "wetb",
        "wetback",
        "wetspot",
        "whacker",
        "whash",
        "whigger",
        "whiskey",
        "whiskeydick",
        "whiskydick",
        "whit",
        "whitenigger",
        "whites",
        "whitetrash",
        "whitey",
        "whiz",
        "whop",
        "whore",
        "whorefucker",
        "whorehouse",
        "wigger",
        "willie",
        "williewanker",
        "willy",
        "wog",
        "wop",
        "wuss",
        "wuzzie",
        "xtc",
        "xxx",
        "yankee",
        "yellowman",
        "zigabo",
        "zipperhead"
    };
}