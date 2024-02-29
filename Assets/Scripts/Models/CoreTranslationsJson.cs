﻿namespace Assets.Scripts.Models
{
    public class CoreTranslationsJson
    {
        public static string json = @"
{
    ""header"": ""Core Translations"",
    ""words"": [
      {
        ""identifier"": ""ERROR"",
        ""english"": ""ERROR"",
        ""russian"": ""Ошибка"",
        ""chinese"": ""错误"",
        ""slovak"": ""Chyba""
      },
      {
        ""identifier"": ""Equipment"",
        ""english"": ""Equipment"",
        ""russian"": ""Aппаратура"",
        ""chinese"": ""设备"",
        ""slovak"": ""Vybavenie""
      },
      {
        ""identifier"": ""FibrousLeaves"",
        ""english"": ""Fibrous leaves"",
        ""russian"": ""Волокнистые листья"",
        ""chinese"": ""纤维叶"",
        ""slovak"": ""Vláknité listy""
      },
      {
        ""identifier"": ""FibrousLeavesDesc"",
        ""english"": ""Basic resource needed to produce higher level items, such as bio fuel."",
        ""russian"": ""Основной съедобный ресурс, необходимый для производства предметов более высокого уровня, таких как биотопливо."",
        ""chinese"": ""这是生产生物燃料等高级物品所需的基本且可食用的资源。"",
        ""slovak"": ""Základný zdroj surovín vužiteľný na výrobu predmetov vyššej úrovne, ako je biopalivo alebo na jedenie.""
      },
      {
        ""identifier"": ""Water"",
        ""english"": ""Water"",
        ""russian"": ""Вода"",
        ""chinese"": ""水"",
        ""slovak"": ""Voda""
      },
      {
        ""identifier"": ""WaterDesc"",
        ""english"": ""This water is not drinkable. Necessary for the production of distilled water."",
        ""russian"": ""Эта вода не пригодна для питья. Необходим для производства дистиллированной воды."",
        ""chinese"": ""这种水不能饮用。生产蒸馏水所必需的。"",
        ""slovak"": ""Táto voda nie je pitná. Nevyhnutná na výrobu destilovanej vody.""
      },
      {
        ""identifier"": ""Biofuel"",
        ""english"": ""Bio fuel"",
        ""russian"": ""Биотопливо"",
        ""chinese"": ""生物燃料"",
        ""slovak"": ""Biopalivo""
      },
      {
        ""identifier"": ""BiofuelDesc"",
        ""english"": ""A common fuel that is easy to produce but depletes quickly. Great for refueling small equipment."",
        ""russian"": ""Обычное топливо, которое легко производить, но быстро истощается. Отлично подходит для заправки небольшой техники."",
        ""chinese"": ""一种易于生产但消耗很快的普通燃料。非常适合为小型设备加油。"",
        ""slovak"": ""Bežné palivo, ktoré sa ľahko vyrába, no rýchlo sa vyčerpá. Skvelé na tankovanie malých zariadení.""
      },
      {
        ""identifier"": ""DistilledWater"",
        ""english"": ""Distilled water"",
        ""russian"": ""Дистиллированная вода"",
        ""chinese"": ""蒸馏水"",
        ""slovak"": ""Destilovaná voda""
      },
      {
        ""identifier"": ""DistilledWaterDesc"",
        ""english"": ""<color=yellow>Water consumption</color>: 0.001/s"",
        ""russian"": ""<color=yellow>Потребление воды</color>: 0.001/s"",
        ""chinese"": ""<color=yellow>用水量</color>: 0.001/s"",
        ""slovak"": ""<color=yellow>Spotreba vody</color>: 0.001/s""
      },
      {
        ""identifier"": ""BatteryCore"",
        ""english"": ""Battery core"",
        ""russian"": ""Ядро батареи"",
        ""chinese"": ""电池芯"",
        ""slovak"": ""Jadro batérie""
      },
      {
        ""identifier"": ""BatteryCoreDesc"",
        ""english"": ""Component necessary for creating energy products."",
        ""russian"": ""Компонент, необходимый для создания энергетических продуктов."",
        ""chinese"": ""该组件是创建能源产品所必需的。"",
        ""slovak"": ""Komponent potrebný na výrobu energetických produktov.""
      },
      {
        ""identifier"": ""Battery"",
        ""english"": ""Battery"",
        ""russian"": ""Батарея"",
        ""chinese"": ""电池"",
        ""slovak"": ""Batéria""
      },
      {
        ""identifier"": ""BatteryDesc"",
        ""english"": ""<color=yellow>Energy consumption</color>: 0.0166/s"",
        ""russian"": ""<color=yellow>Потребление энергии</color>: 0.0166/s"",
        ""chinese"": ""<color=yellow>能源消耗</color>: 0.0166/s"",
        ""slovak"": ""<color=yellow>Spotreba energie</color>: 0.0166/s""
      },
      {
        ""identifier"": ""OxygenTank"",
        ""english"": ""Oxygen tank"",
        ""russian"": ""Кислородный баллон"",
        ""chinese"": ""氧气罐"",
        ""slovak"": ""Kyslíková fľaša""
      },
      {
        ""identifier"": ""OxygenTankDesc"",
        ""english"": ""<color=yellow>Oxygen consumption</color>: 0.001/s"",
        ""russian"": ""<color=yellow>Потребление кислорода</color>: 0.001/s"",
        ""chinese"": ""<color=yellow>耗氧量</color>: 0.001/s"",
        ""slovak"": ""<color=yellow>Spotreba kyslíka</color>: 0.001/s""
      },
      {
        ""identifier"": ""LatexFoam"",
        ""english"": ""Latex foam"",
        ""russian"": ""Латексная пена"",
        ""chinese"": ""乳胶泡沫"",
        ""slovak"": ""Latexová pena""
      },
      {
        ""identifier"": ""LatexFoamDesc"",
        ""english"": ""A natural latex foam, derived from the sap of trees."",
        ""russian"": ""Натуральная латексная пена, полученная из сока деревьев."",
        ""chinese"": ""这是一种天然乳胶泡沫，源自树液。"",
        ""slovak"": ""Prírodná latexová pena získaná z miazgy stromov.""
      },
      {
        ""identifier"": ""ProteinBeans"",
        ""english"": ""Protein beans"",
        ""russian"": ""Протеиновые бобы"",
        ""chinese"": ""蛋白豆"",
        ""slovak"": ""Proteínové bobule""
      },
      {
        ""identifier"": ""ProteinBeansDesc"",
        ""english"": ""Edible resource that can be later processed."",
        ""russian"": ""Съедобный ресурс, который можно позже переработать."",
        ""chinese"": ""这是一种可以稍后加工的可食用资源。"",
        ""slovak"": ""Jedlý zdroj, ktorý možno neskôr spracovať.""
      },
      {
        ""identifier"": ""ProteinPowder"",
        ""english"": ""Protein powder"",
        ""russian"": ""Протеиновый порошок"",
        ""chinese"": ""蛋白粉"",
        ""slovak"": ""Proteínový prášok""
      },
      {
        ""identifier"": ""ProteinPowderDesc"",
        ""english"": ""Standard supplement that can be used in medicine or food."",
        ""russian"": ""Стандартная добавка, которую можно использовать в медицине или пище."",
        ""chinese"": ""这是一种常见的补充剂，可用于药物或食品。"",
        ""slovak"": ""Štandardný doplnok, ktorý možno použiť v medicíne alebo potravinách.""
      },
      {
        ""identifier"": ""BiomassLeaves"",
        ""english"": ""Biomass(leaves)"",
        ""russian"": ""Биомасса (листья)"",
        ""chinese"": ""生物量（树叶）"",
        ""slovak"": ""Biomasa(listy)""
      },
      {
        ""identifier"": ""BiomassLeavesDesc"",
        ""english"": ""Processed amount of biological material."",
        ""russian"": ""Переработанное количество биологического материала."",
        ""chinese"": ""这是经过加工的生物材料。"",
        ""slovak"": ""Spracovaný množstvo biologického materiálu.""
      },
      {
        ""identifier"": ""BiomassWood"",
        ""english"": ""Biomass(wood)"",
        ""russian"": ""Биомасса (древесина)"",
        ""chinese"": ""生物量（木材）"",
        ""slovak"": ""Biomasa(drevo)""
      },
      {
        ""identifier"": ""BiomassWoodDesc"",
        ""english"": ""Processed amount of biological material."",
        ""russian"": ""Переработанное количество биологического материала."",
        ""chinese"": ""这是经过加工的生物材料。"",
        ""slovak"": ""Spracovaný množstvo biologického materiálu.""
      },
      {
        ""identifier"": ""BioOil"",
        ""english"": ""Bio oil"",
        ""russian"": ""Биомасло"",
        ""chinese"": ""生物油"",
        ""slovak"": ""Bio olej""
      },
      {
        ""identifier"": ""BioOilDesc"",
        ""english"": ""Transparent, greasy substance made from natural resources."",
        ""russian"": ""Прозрачное, жирное вещество, изготовленное из природных ресурсов."",
        ""chinese"": ""这种油是一种由自然资源制成的透明油状物质。"",
        ""slovak"": ""Transparentná, mastná látka vyrobená z prírodných zdrojov.""
      },
      {
        ""identifier"": ""BuildingType"",
        ""english"": ""Building type:"",
        ""russian"": ""Тип здания:"",
        ""chinese"": ""建筑类型："",
        ""slovak"": ""Typ budovy:""
      },
      {
        ""identifier"": ""Consumption"",
        ""english"": ""Consumption:"",
        ""russian"": ""Потребление:"",
        ""chinese"": ""消耗："",
        ""slovak"": ""Spotreba:""
      },
      {
        ""identifier"": ""Output"",
        ""english"": ""Output:"",
        ""russian"": ""Выход:"",
        ""chinese"": ""建筑输出："",
        ""slovak"": ""Výkon:""
      },
      {
        ""identifier"": ""Materials"",
        ""english"": ""Materials:"",
        ""russian"": ""Материалы:"",
        ""chinese"": ""所需材料："",
        ""slovak"": ""Materiály:""
      },
      {
        ""identifier"": ""BiofuelGenerator"",
        ""english"": ""Bio fuel generator"",
        ""russian"": ""Генератор биотоплива"",
        ""chinese"": ""生物燃料发电机"",
        ""slovak"": ""Generátor biopalív""
      },
      {
        ""identifier"": ""BiofuelGeneratorDesc"",
        ""english"": ""Consumes  <color=yellow>Bio fuel </color>to generate electricity."",
        ""russian"": ""Потребляет <color=yellow>биотопливо </color> для выработки электроэнергии."",
        ""chinese"": ""消耗<color=yellow>生物燃料</color>来发电。"",
        ""slovak"": ""Na výrobu elektriny spotrebuje <color=yellow>biopalivo </color>.""
      },
      {
        ""identifier"": ""WaterPump"",
        ""english"": ""Water pump"",
        ""russian"": ""Bодяной насос"",
        ""chinese"": ""抽水机"",
        ""slovak"": ""Vodné čerpadlo""
      },
      {
        ""identifier"": ""WaterPumpDesc"",
        ""english"": ""Creates <color=yellow>10 Water</color> from the planet's reserves every 10 seconds."",
        ""russian"": ""Создает <color=yellow>10 единиц воды</color> из запасов планеты каждые 10 секунд."",
        ""chinese"": ""每 10 秒从地球储备中产生 <color=yellow>10 水</color>。"",
        ""slovak"": ""Ťaží <color=yellow>10 vody</color> zo zásob planéty každých 10 sekúnd.""
      },
      {
        ""identifier"": ""FibrousPlantField"",
        ""english"": ""Fibrous plant field"",
        ""russian"": ""Поле волокнистых растений"",
        ""chinese"": ""纤维植物领域"",
        ""slovak"": ""Pole vláknitých rastlín""
      },
      {
        ""identifier"": ""FibrousPlantFieldDesc"",
        ""english"": ""Allows you to harvest <color=yellow>fibrous leaves</color>."",
        ""russian"": ""Позволяет собирать <color=yellow>волокнистые листья</color>."",
        ""chinese"": ""这块田地可以让你收获<color=yellow>纤维状叶子</color>。"",
        ""slovak"": ""Umožňuje zber <color=yellow>Vláknitých listov</color>.""
      },
      {
        ""identifier"": ""Steam"",
        ""english"": ""Steam"",
        ""russian"": ""Пар"",
        ""chinese"": ""蒸汽"",
        ""slovak"": ""Para""
      },
      {
        ""identifier"": ""SteamDesc"",
        ""english"": ""Substance containing water in the gas phase created by the evaporation process."",
        ""russian"": ""Вещество, содержащее воду в газовой фазе, созданной в процессе испарения."",
        ""chinese"": ""这是一种通过蒸发过程产生的气相含水物质。"",
        ""slovak"": ""Látka obsahujúca vodu v plynnej fáze vytvorenej procesom odparovania.""
      },
      {
        ""identifier"": ""SteamGenerator"",
        ""english"": ""Steam generator"",
        ""russian"": ""Парогенератор"",
        ""chinese"": ""蒸汽发生器"",
        ""slovak"": ""Parný generátor""
      },
      {
        ""identifier"": ""SteamGeneratorDesc"",
        ""english"": ""Produces <color=yellow>energy</color> from steam."",
        ""russian"": ""Производит <color=yellow>энергию</color> из пара."",
        ""chinese"": ""该发电机从蒸汽产生<color=yellow>能量</color>。"",
        ""slovak"": ""Produkuje <color=yellow>energiu</color> z pary.""
      },
      {
        ""identifier"": ""Boiler"",
        ""english"": ""Boiler"",
        ""russian"": ""Котел"",
        ""chinese"": ""锅炉"",
        ""slovak"": ""Kotol""
      },
      {
        ""identifier"": ""BoilerDesc"",
        ""english"": ""Produces <color=yellow>steam</color> by heating water."",
        ""russian"": ""Нагревает воду для образования <color=yellow>пара</color>."",
        ""chinese"": ""该锅炉通过加热水产生<color=yellow>蒸汽</color>。"",
        ""slovak"": ""Vytvára <color=yellow>paru</color> ohrevom vody.""
      },
      {
        ""identifier"": ""ResearchDevice"",
        ""english"": ""Research device"",
        ""russian"": ""Исследовательское устройство"",
        ""chinese"": ""研究装置"",
        ""slovak"": ""Výskumné zariadenie""
      },
      {
        ""identifier"": ""ResearchDeviceDesc"",
        ""english"": ""Generates <color=yellow>research points</color> at the cost of electricity."",
        ""russian"": ""Устройство, генерирующее <color=yellow>точки исследования</color> с помощью электричества."",
        ""chinese"": ""这会利用电力产生<color=yellow>研究点</color>。"",
        ""slovak"": ""Generuje <color=yellow>výskumné body</color> za cenu elektriny.""
      },
      {
        ""identifier"": ""Wood"",
        ""english"": ""Wood"",
        ""russian"": ""Древесина"",
        ""chinese"": ""木头"",
        ""slovak"": ""Drevo""
      },
      {
        ""identifier"": ""WoodDesc"",
        ""english"": ""Basic construction material."",
        ""russian"": ""Основной строительный материал."",
        ""chinese"": ""这是建筑的基本材料。"",
        ""slovak"": ""Základný stavebný materiál.""
      },
      {
        ""identifier"": ""IronOre"",
        ""english"": ""Iron ore"",
        ""russian"": ""Железная руда"",
        ""chinese"": ""铁矿"",
        ""slovak"": ""Železná ruda""
      },
      {
        ""identifier"": ""IronOreDesc"",
        ""english"": ""Basic material for construction or smelting."",
        ""russian"": ""Основной ресурс для строительства или плавки."",
        ""chinese"": ""这是建筑或冶炼的基本资源。"",
        ""slovak"": ""Základný materiál pre stavbu alebo tavenie.""
      },
      {
        ""identifier"": ""Coal"",
        ""english"": ""Coal"",
        ""russian"": ""Уголь"",
        ""chinese"": ""煤炭"",
        ""slovak"": ""Uhlie""
      },
      {
        ""identifier"": ""CoalDesc"",
        ""english"": ""A vital source of energy mainly for industrial use."",
        ""russian"": ""Жизненно важный источник энергии, главным образом для промышленного использования."",
        ""chinese"": ""煤炭是重要的能源，主要用于工业用途。"",
        ""slovak"": ""Životne dôležitý zdroj energie, hlavne pre priemyselné využitie.""
      },
      {
        ""identifier"": ""IronBeam"",
        ""english"": ""Iron beam"",
        ""russian"": ""Железная балка"",
        ""chinese"": ""铁梁"",
        ""slovak"": ""Železný trám""
      },
      {
        ""identifier"": ""IronBeamDesc"",
        ""english"": ""Common component used to construct buildings frames."",
        ""russian"": ""Общий компонент, используемый для построения каркасов зданий."",
        ""chinese"": ""铁梁是用于建造建筑框架的标准构件。"",
        ""slovak"": ""Bežný komponent používaný na stavbu rámov budov.""
      },
      {
        ""identifier"": ""Electricity"",
        ""english"": ""Electricity"",
        ""russian"": ""Электричество"",
        ""chinese"": ""电力"",
        ""slovak"": ""Elektrina""
      },
      {
        ""identifier"": ""UVIndex"",
        ""english"": ""UV Index"",
        ""russian"": ""УФ-индекс"",
        ""chinese"": ""紫外线指数"",
        ""slovak"": ""UV index""
      },
      {
        ""identifier"": ""SunriseSunset"",
        ""english"": ""Sunrise / Sunset"",
        ""russian"": ""Восход / Закат"",
        ""chinese"": ""日出 / 日落"",
        ""slovak"": ""Východ / Západ""
      },
      {
        ""identifier"": ""Sunrise"",
        ""english"": ""Sunrise"",
        ""russian"": ""Восход"",
        ""chinese"": ""日出"",
        ""slovak"": ""Východ""
      },
      {
        ""identifier"": ""Sunset"",
        ""english"": ""Sunset"",
        ""russian"": ""Закат"",
        ""chinese"": ""日落"",
        ""slovak"": ""Západ""
      },
      {
        ""identifier"": ""Sunny"",
        ""english"": ""Sunny"",
        ""russian"": ""Солнечно"",
        ""chinese"": ""晴朗"",
        ""slovak"": ""Slnečno""
      },
      {
        ""identifier"": ""BuildingsStatistics"",
        ""english"": ""Buildings statistics"",
        ""russian"": ""Статистика построек"",
        ""chinese"": ""建筑物统计"",
        ""slovak"": ""Štatistika budov""
      },
      {
        ""identifier"": ""PlayerLevel"",
        ""english"": ""Player level"",
        ""russian"": ""Уровень игрока"",
        ""chinese"": ""球员等级"",
        ""slovak"": ""Úroveň hráča""
      },
      {
        ""identifier"": ""Survive"",
        ""english"": ""Survive"",
        ""russian"": ""Пережить"",
        ""chinese"": ""生存"",
        ""slovak"": ""Prežitie""
      },
      {
        ""identifier"": ""SignIn"",
        ""english"": ""Sign in"",
        ""russian"": ""Войти"",
        ""chinese"": ""登入"",
        ""slovak"": ""Prihlásiť sa""
      },
      {
        ""identifier"": ""Login"",
        ""english"": ""Log in"",
        ""russian"": ""Авторизоваться"",
        ""chinese"": ""登录"",
        ""slovak"": ""Prihlásiť sa""
      },
      {
        ""identifier"": ""NewUser"",
        ""english"": ""new user"",
        ""russian"": ""новый пользователь"",
        ""chinese"": ""新用戶"",
        ""slovak"": ""nový užívateľ""
      },
      {
        ""identifier"": ""RecoverPasswordUsername"",
        ""english"": ""Recover password / username"",  
        ""russian"": ""Восстановить пароль / имя пользователя"",
        ""chinese"": ""恢复密码 / 用户名"",
        ""slovak"": ""Obnoviť heslo / užívateľské meno""
      },
      {
        ""identifier"": ""NameEmail"",
        ""english"": ""Name / Email"",
        ""russian"": ""Имя / Электронная почта"",
        ""chinese"": ""姓名 / 電子郵件"",
        ""slovak"": ""Meno / Email""
      },
      {
        ""identifier"": ""CreateAccount"",
        ""english"": ""Create account"",
        ""russian"": ""Зарегистрироваться"",
        ""chinese"": ""创建账户"",
        ""slovak"": ""Vytvoriť účet""
      },
      {
        ""identifier"": ""Email"",
        ""english"": ""e-mail"",
        ""russian"": ""E-мейл"",
        ""chinese"": ""电邮"",
        ""slovak"": ""e-mail""
      },
      {
        ""identifier"": ""Username"",
        ""english"": ""username"",
        ""russian"": ""имя"",
        ""chinese"": ""用户名"",
        ""slovak"": ""meno""
      },
      {
        ""identifier"": ""EmailOrUsername"",
        ""english"": ""email or username"",
        ""russian"": ""электронная почта или имя"",
        ""chinese"": ""电子邮件或用户名"",
        ""slovak"": ""e-mail alebo meno""
      },
      {
        ""identifier"": ""Password"",
        ""english"": ""password"",
        ""russian"": ""пароль"",
        ""chinese"": ""密码"",
        ""slovak"": ""heslo""
      },
      {
        ""identifier"": ""VerificationCode"",
        ""english"": ""verification code"",
        ""russian"": ""код подтверждения"",
        ""chinese"": ""验证码"",
        ""slovak"": ""overovací kód""
      },

      {
        ""identifier"": ""Start"",
        ""english"": ""Start"",
        ""russian"": ""Начать"",
        ""chinese"": ""开始"",
        ""slovak"": ""Začať""
      },
      {
        ""identifier"": ""Back"",
        ""english"": ""Back"",
        ""russian"": ""Назад"",
        ""chinese"": ""后退"",
        ""slovak"": ""Späť""
      },
      {
        ""identifier"": ""Next"",
        ""english"": ""Next"",
        ""russian"": ""Далее"",
        ""chinese"": ""下一个"",
        ""slovak"": ""Ďalší""
      },
      {
        ""identifier"": ""Recover"",
        ""english"": ""Recover"",
        ""russian"": ""Восстановить"",
        ""chinese"": ""恢复"",
        ""slovak"": ""Obnoviť""
        },
        {
        ""identifier"": ""Send"",
        ""english"": ""Send"",
        ""russian"": ""Отправить"",
        ""chinese"": ""发送"",
        ""slovak"": ""Odoslať""
        },

      {
        ""identifier"": ""Strength"",
        ""english"": ""Strength:"",
        ""russian"": ""Сила:"",
        ""chinese"": ""力量:"",
        ""slovak"": ""Sila:""
      },
      {
        ""identifier"": ""Perception"",
        ""english"": ""Perception:"",
        ""russian"": ""Восприятие:"",
        ""chinese"": ""知觉:"",
        ""slovak"": ""Vnímanie:""
      },
      {
        ""identifier"": ""Intelligence"",
        ""english"": ""Intelligence:"",
        ""russian"": ""Интеллект:"",
        ""chinese"": ""智力:"",
        ""slovak"": ""Inteligencia:""
      },
      {
        ""identifier"": ""Agility"",
        ""english"": ""Agility:"",
        ""russian"": ""Ловкость:"",
        ""chinese"": ""敏捷:"",
        ""slovak"": ""Obratnosť:""
      },
      {
        ""identifier"": ""Charisma"",
        ""english"": ""Charisma:"",
        ""russian"": ""Харизма:"",
        ""chinese"": ""魅力:"",
        ""slovak"": ""Charizma:""
      },
      {
        ""identifier"": ""Willpower"",
        ""english"": ""Willpower:"",
        ""russian"": ""Сила воли:"",
        ""chinese"": ""意志:"",
        ""slovak"": ""Sila vôle:""
      },
      {
        ""identifier"": ""Stats"",
        ""english"": ""Stats"",
        ""russian"": ""Статистика"",
        ""chinese"": ""统计数据"",
        ""slovak"": ""Vlastnosti""
      },
      {
        ""identifier"": ""Skills"",
        ""english"": ""Skills"",
        ""russian"": ""Навыки"",
        ""chinese"": ""技能"",
        ""slovak"": ""Zručnosti""
      },
      {
        ""identifier"": ""Status"",
        ""english"": ""Status"",
        ""russian"": ""Cтатус"",
        ""chinese"": ""地位"",
        ""slovak"": ""Stav""
      },
      {
        ""identifier"": ""StatusReport"",
        ""english"": ""Status report"",
        ""russian"": ""Отчет"",
        ""chinese"": ""状况报告"",
        ""slovak"": ""Hlásenie správy""
      },
      {
        ""identifier"": ""StatusReportDesc"",
        ""english"": ""Your spacecraft shook violently as it descended towards the unknown planet's surface. The crew held onto their seats, trying to brace themselves for the inevitable crash. With a loud bang, the ship hit the ground, sending debris flying everywhere. You've been able to salvage some resources but found the rest of the crew dead."",
        ""russian"": ""Космический корабль сильно трясло, когда он спускался к поверхности неизвестной планеты. Экипаж держался за свои места, пытаясь подготовиться к неизбежному столкновению. С громким грохотом корабль ударился о землю, разбрасывая во все стороны обломки. Вам удалось спасти некоторые материалы, но больше никого не было в живых."",
        ""chinese"": ""飞船剧烈一晃，降落在了一颗未知星球的地表。机组人员紧紧抓住他们的座位，试图为不可避免的坠机做准备。一声巨响，船身重重的撞在了地面上，碎屑飞扬。您已经能够挽救一些材料，但没有其他人幸存下来。"",
        ""slovak"": ""Kozmická loď sa prudko otriasala, keď klesala k povrchu neznámej planéty. Posádka sa držala svojich sedadiel a snažila sa pripraviť na nevyhnutný náraz. S hlasným treskom loď dopadla na zem a trosky lietali všade. Podarilo sa vám pozbierař nejaké materiály, no nikto iný neostal na žive.""
      },
      {
        ""identifier"": ""FriendsPanel"",
        ""english"": ""Friends panel"",
        ""russian"": ""Панель друзей"",
        ""chinese"": ""显示好友菜单"",
        ""slovak"": ""Panel priateľov""
      },
      {
        ""identifier"": ""CrashLanding"",
        ""english"": ""Crash landing"",
        ""russian"": ""Аварийная посадка"",
        ""chinese"": ""迫降"",
        ""slovak"": ""Nárazové pristátie""
      },
      {
        ""identifier"": ""FriendRequestList"",
        ""english"": ""Friend request list"",
        ""russian"": ""Запросы в друзья"",
        ""chinese"": ""好友请求列表"",
        ""slovak"": ""Žiadosti o priateľstvá""
      },
      {
        ""identifier"": ""Submit"",
        ""english"": ""Submit"",
        ""russian"": ""Подтверждать"",
        ""chinese"": ""确认"",
        ""slovak"": ""Potvrdiť""
      },
      {
        ""identifier"": ""SyncingData"",
        ""english"": ""SYNCING DATA"",
        ""russian"": ""Загрузка данных"",
        ""chinese"": ""加载数据中"",
        ""slovak"": ""NAČÍTAVAM DATA""
      },
      {
        ""identifier"": ""InitializingGame"",
        ""english"": ""INITIALIZING GAME..."",
        ""russian"": ""НИЦИАЛИЗАЦИЯ ИГРЫ..."",
        ""chinese"": ""游戏初始化中..."",
        ""slovak"": ""INICIALIZÁCIA HRY...""
      },
      {
        ""identifier"": ""Registered"",
        ""english"": ""Registered"",
        ""russian"": ""Зарегистрирован"",
        ""chinese"": ""挂号的"",
        ""slovak"": ""Registrovaný""
      },
      {
        ""identifier"": ""NotRegistered"",
        ""english"": ""Not registered"",
        ""russian"": ""Не зарегистрирован"",
        ""chinese"": ""未注册"",
        ""slovak"": ""Neregistrovaný""
      },
      {
        ""identifier"": ""Objective"",
        ""english"": ""Objective:"",
        ""russian"": ""Цель:"",
        ""chinese"": ""客观的:"",
        ""slovak"": ""Cieľ:""
      },
      {
        ""identifier"": ""Hunger"",
        ""english"": ""Hunger"",
        ""russian"": ""Голод"",
        ""chinese"": ""饥饿"",
        ""slovak"": ""Hlad""
      },
      {
        ""identifier"": ""StartNewGame"",
        ""english"": ""Start new game"",
        ""russian"": ""Начать новую игру"",
        ""chinese"": ""开始新游戏"",
        ""slovak"": ""Začať novú hru""
      },
      {
        ""identifier"": ""EmptySlot1"",
        ""english"": ""Empty slot 1"",
        ""russian"": ""Пустой слот 1"",
        ""chinese"": ""空插槽 1"",
        ""slovak"": ""Prázdny slot 1""
      },
      {
        ""identifier"": ""EmptySlot2"",
        ""english"": ""Empty slot 2"",
        ""russian"": ""Пустой слот 2"",
        ""chinese"": ""空插槽 2"",
        ""slovak"": ""Prázdny slot 2""
      },
      {
        ""identifier"": ""EmptySlot3"",
        ""english"": ""Empty slot 3"",
        ""russian"": ""Пустой слот 3"",
        ""chinese"": ""空插槽 3"",
        ""slovak"": ""Prázdny slot 3""
      },
      {
        ""identifier"": ""EmptySlot4"",
        ""english"": ""Empty slot 4"",
        ""russian"": ""Пустой слот 4"",
        ""chinese"": ""空插槽 4"",
        ""slovak"": ""Prázdny slot 4""
      },
      {
        ""identifier"": ""PrivacyPolicy"",
        ""english"": ""Privacy policy"",
        ""russian"": ""Политика конфиденциальности"",
        ""chinese"": ""隐私政策"",
        ""slovak"": ""Zásady ochrany osobných údajov""
      },
      {
        ""identifier"": ""Blueprint"",
        ""english"": "" - (Blueprint)"",
        ""russian"": "" - (План)"",
        ""chinese"": "" - (蓝图)"",
        ""slovak"": "" - (Plán)""
      },
      {
        ""identifier"": ""ALLBLUEPRINTS"",
        ""english"": ""All blueprints"",
        ""russian"": ""Все чертежи"",
        ""chinese"": ""所有蓝图"",
        ""slovak"": ""Všetky plány""
      },
      {
        ""identifier"": ""ALLITEMS"",
        ""english"": ""All items"",
        ""russian"": ""Все предметы"",
        ""chinese"": ""万物"",
        ""slovak"": ""Všetky veci""
      },
      {
        ""identifier"": ""BASIC"",
        ""english"": ""Basic"",
        ""russian"": ""Основные"",
        ""chinese"": ""基础产品"",
        ""slovak"": ""Základné""
      },
      {
        ""identifier"": ""PROCESSED"",
        ""english"": ""Processed"",
        ""russian"": ""Обработанные"",
        ""chinese"": ""处理过的产品"",
        ""slovak"": ""Spracované""
      },
      {
        ""identifier"": ""ENHANCED"",
        ""english"": ""Enhanced"",
        ""russian"": ""Улучшенные"",
        ""chinese"": ""强化产品"",
        ""slovak"": ""Upravené""
      },
      {
        ""identifier"": ""ASSEMBLED"",
        ""english"": ""Assembled"",
        ""russian"": ""Собранные"",
        ""chinese"": ""组装产品"",
        ""slovak"": ""Zostavené""
      },
      {
        ""identifier"": ""BUILDINGS"",
        ""english"": ""Buildings"",
        ""russian"": ""Здания"",
        ""chinese"": ""建筑物"",
        ""slovak"": ""Budovy""
      },
      {
        ""identifier"": ""ALLTYPES"",
        ""english"": ""All Types"",
        ""russian"": ""Все типы"",
        ""chinese"": ""所有类型"",
        ""slovak"": ""Všetky typy""
      },
      {
        ""identifier"": ""PLANTS"",
        ""english"": ""Plants"",
        ""russian"": ""Растения"",
        ""chinese"": ""植物"",
        ""slovak"": ""Rastliny""
      },
      {
        ""identifier"": ""LIQUID"",
        ""english"": ""Liquid"",
        ""russian"": ""Жидкость"",
        ""chinese"": ""液体"",
        ""slovak"": ""Tekutiny""
      },
      {
        ""identifier"": ""MINERALS"",
        ""english"": ""Minerals"",
        ""russian"": ""Минералы"",
        ""chinese"": ""矿物质"",
        ""slovak"": ""Minerály""
      },
      {
        ""identifier"": ""GAS"",
        ""english"": ""Gas"",
        ""russian"": ""Газ"",
        ""chinese"": ""气体"",
        ""slovak"": ""Plyny""
      },
      {
        ""identifier"": ""METALS"",
        ""english"": ""Metals"",
        ""russian"": ""Металлы"",
        ""chinese"": ""金属"",
        ""slovak"": ""Kovy""
      },
      {
        ""identifier"": ""FOAM"",
        ""english"": ""Foam"",
        ""russian"": ""Пена"",
        ""chinese"": ""泡沫"",
        ""slovak"": ""Pena""
      },
      {
        ""identifier"": ""POWDERS"",
        ""english"": ""Powders"",
        ""russian"": ""Порошки"",
        ""chinese"": ""粉末"",
        ""slovak"": ""Prášky""
      },
      {
        ""identifier"": ""FLESH"",
        ""english"": ""Flesh"",
        ""russian"": ""Плоть"",
        ""chinese"": ""肉体"",
        ""slovak"": ""Mäso""
      },
      {
        ""identifier"": ""WEAPONS"",
        ""english"": ""Weapons"",
        ""russian"": ""Оружие"",
        ""chinese"": ""武器"",
        ""slovak"": ""Zbrane""
      },
      {
        ""identifier"": ""ARMOR"",
        ""english"": ""Armor"",
        ""russian"": ""Броня"",
        ""chinese"": ""盔甲"",
        ""slovak"": ""Zbroje""
      },
      {
        ""identifier"": ""HELMET"",
        ""english"": ""Helmet"",
        ""russian"": ""Шлем"",
        ""chinese"": ""头盔"",
        ""slovak"": ""Prilba""
      },
      {
        ""identifier"": ""SUIT"",
        ""english"": ""Suit"",
        ""russian"": ""Костюм"",
        ""chinese"": ""太空服"",
        ""slovak"": ""Oblek""
      },
      {
        ""identifier"": ""FABRICATOR"",
        ""english"": ""Fabricator"",
        ""russian"": ""Фабрикатор"",
        ""chinese"": ""制造装置"",
        ""slovak"": ""Výrobník""
      },
      {
        ""identifier"": ""ENERGY"",
        ""english"": ""Energy"",
        ""russian"": ""Энергия"",
        ""chinese"": ""能源"",
        ""slovak"": ""Energie""
      },
      {
        ""identifier"": ""OXYGEN"",
        ""english"": ""Oxygen"",
        ""russian"": ""Кислород"",
        ""chinese"": ""氧气"",
        ""slovak"": ""Kyslík""
      },
      {
        ""identifier"": ""MINING"",
        ""english"": ""Mining"",
        ""russian"": ""Добыча"",
        ""chinese"": ""采矿"",
        ""slovak"": ""Ťažba""
      },
      {
        ""identifier"": ""BAG"",
        ""english"": ""Bag"",
        ""russian"": ""Сумка"",
        ""chinese"": ""袋子"",
        ""slovak"": ""Taška""
      },
      {
        ""identifier"": ""AGRICULTURE"",
        ""english"": ""Agriculture"",
        ""russian"": ""Cельское хозяйство"",
        ""chinese"": ""农业设施"",
        ""slovak"": ""Poľnohospodárstvo""
      },
      {
        ""identifier"": ""PUMPINGFACILITY"",
        ""english"": ""Pumping facility"",
        ""russian"": ""Насосная установка"",
        ""chinese"": ""抽水设施"",
        ""slovak"": ""Čerpacie zariadenie""
      },
      {
        ""identifier"": ""FACTORY"",
        ""english"": ""Factory"",
        ""russian"": ""Фабрика"",
        ""chinese"": ""工厂"",
        ""slovak"": ""Fabrika""
      },
      {
        ""identifier"": ""COMMFACILITY"",
        ""english"": ""Comm facility"",
        ""russian"": ""Средства связи"",
        ""chinese"": ""通讯设施"",
        ""slovak"": ""Komunikačné zariadenie""
      },
      {
        ""identifier"": ""LABORATORY"",
        ""english"": ""Laboratory"",
        ""russian"": ""Лаборатория"",
        ""chinese"": ""实验室"",
        ""slovak"": ""Laboratórium""
      },
      {
        ""identifier"": ""STORAGEHOUSE"",
        ""english"": ""Storage house"",
        ""russian"": ""Складское помещение"",
        ""chinese"": ""储藏室"",
        ""slovak"": ""Skladový dom""
      },
      {
        ""identifier"": ""NAVALFACILITY"",
        ""english"": ""Naval facility"",
        ""russian"": ""Военно-морской объект"",
        ""chinese"": ""海军设施"",
        ""slovak"": ""Námorné zariadenie""
      },
      {
        ""identifier"": ""OXYGENFACILITY"",
        ""english"": ""Oxygen facility"",
        ""russian"": ""Кислородная установка"",
        ""chinese"": ""制氧设施"",
        ""slovak"": ""Kyslíkové zariadenie""
      },
      {
        ""identifier"": ""AVIATIONFACILITY"",
        ""english"": ""Aviation facility"",
        ""russian"": ""Авиационный объект"",
        ""chinese"": ""航空设施"",
        ""slovak"": ""Letecké zariadenie""
      },
      {
        ""identifier"": ""HEATINGFACILITY"",
        ""english"": ""Heating facility"",
        ""russian"": ""Отопительная установка"",
        ""chinese"": ""供暖设施"",
        ""slovak"": ""Vykurovacie zariadenie""
      },
      {
        ""identifier"": ""COOLINGFACILITY"",
        ""english"": ""Cooling facility"",
        ""russian"": ""Охлаждающая установка"",
        ""chinese"": ""冷却设施"",
        ""slovak"": ""Chladiace zariadenie""
      },
      {
        ""identifier"": ""POWERPLANT"",
        ""english"": ""Powerplant"",
        ""russian"": ""Электростанция"",
        ""chinese"": ""发电厂"",
        ""slovak"": ""Elektráreň""
      },
      {
        ""identifier"": ""OXYGENSTATION"",
        ""english"": ""Oxygen station"",
        ""russian"": ""Кислородная станция"",
        ""chinese"": ""氧气站"",
        ""slovak"": ""Kyslíková stanica""
      },
      {
        ""identifier"": ""MININGRIG"",
        ""english"": ""Mining rig"",
        ""russian"": ""Горная установка"",
        ""chinese"": ""采矿设施"",
        ""slovak"": ""Ťažobné zariadenie""
      },
      {
        ""identifier"": ""ALLCLASSES"",
        ""english"": ""All classes"",
        ""russian"": ""Все классы"",
        ""chinese"": ""所有等级"",
        ""slovak"": ""Všetky triedy""
      },
      {
        ""identifier"": ""CLASS-F"",
        ""english"": ""Class-F"",
        ""russian"": ""Класс-F"",
        ""chinese"": ""等级-F"",
        ""slovak"": ""Trieda-F""
      },
      {
        ""identifier"": ""CLASS-E"",
        ""english"": ""Class-E"",
        ""russian"": ""Класс-E"",
        ""chinese"": ""等级-E"",
        ""slovak"": ""Trieda-E""
      },
      {
        ""identifier"": ""CLASS-D"",
        ""english"": ""Class-D"",
        ""russian"": ""Класс-D"",
        ""chinese"": ""等级-D"",
        ""slovak"": ""Trieda-D""
      },
      {
        ""identifier"": ""CLASS-C"",
        ""english"": ""Class-C"",
        ""russian"": ""Класс-C"",
        ""chinese"": ""等级-C"",
        ""slovak"": ""Trieda-C""
      },
      {
        ""identifier"": ""CLASS-B"",
        ""english"": ""Class-B"",
        ""russian"": ""Класс-B"",
        ""chinese"": ""等级-B"",
        ""slovak"": ""Trieda-B""
      },
      {
        ""identifier"": ""CLASS-A"",
        ""english"": ""Class-A"",
        ""russian"": ""Класс-A"",
        ""chinese"": ""等级-A"",
        ""slovak"": ""Trieda-A""
      },
      {
        ""identifier"": ""CLASS-S"",
        ""english"": ""Class-S"",
        ""russian"": ""Класс-S"",
        ""chinese"": ""等级-S"",
        ""slovak"": ""Trieda-S""
      },
      {
        ""identifier"": ""Exploration"",
        ""english"": ""Exploration"",
        ""russian"": ""Исследование"",
        ""chinese"": ""勘探"",
        ""slovak"": ""Prieskum""
      },
      {
        ""identifier"": ""Production"",
        ""english"": ""Production"",
        ""russian"": ""Производство"",
        ""chinese"": ""生产"",
        ""slovak"": ""Výroba""
      },
      {
        ""identifier"": ""Inventory"",
        ""english"": ""Inventory"",
        ""russian"": ""Инвентарь"",
        ""chinese"": ""存货"",
        ""slovak"": ""Inventár""
      },
      {
        ""identifier"": ""Base"",
        ""english"": ""Base"",
        ""russian"": ""База"",
        ""chinese"": ""基地"",
        ""slovak"": ""Základňa""
      },
      {
        ""identifier"": ""Overview"",
        ""english"": ""Overview"",
        ""russian"": ""Обзор"",
        ""chinese"": ""概述"",
        ""slovak"": ""Prehľad""
      },
      {
        ""identifier"": ""Planning"",
        ""english"": ""Planning"",
        ""russian"": ""Планирование"",
        ""chinese"": ""规划"",
        ""slovak"": ""Plánovanie""
      },
      {
        ""identifier"": ""LifeSupport"",
        ""english"": ""Life support"",
        ""russian"": ""жизнеобеспечение"",
        ""chinese"": ""生命保障"",
        ""slovak"": ""Životná podpora""
      },
      {
        ""identifier"": ""ManualProduction"",
        ""english"": ""Manual production"",
        ""russian"": ""Ручное производство"",
        ""chinese"": ""手工制作"",
        ""slovak"": ""Manuálna výroba""
      },
      {
        ""identifier"": ""PlanetIndex"",
        ""english"": ""Planet index"",
        ""russian"": ""Индекс планеты"",
        ""chinese"": ""行星指数"",
        ""slovak"": ""Planetárny index""
      },
      {
        ""identifier"": ""PlanetIndexDesc"",
        ""english"": ""Represents the overall planetary quality index by calculating each planetary resource and its overall impact on human well-being."",
        ""russian"": ""Представляет общий планетарный индекс качества путем расчета каждого ресурса планеты и его общего влияния на благосостояние людей."",
        ""chinese"": ""该指数通过计算地球所具有的每一种状况及其对人类福祉的总体影响来代表整体星球质量指数。"",
        ""slovak"": ""Predstavuje celkový index planetárnej kvality výpočtom každého planetárneho zdroja a jeho celkového vplyvu na ľudské blaho.""
      },
      {
        ""identifier"": ""Objective"",
        ""english"": ""Objective"",
        ""russian"": ""Задача"",
        ""chinese"": ""任务"",
        ""slovak"": ""Úloha""
      },
      {
        ""identifier"": ""CraftBatteryDesc"",
        ""english"": ""Your suit needs energy to function. Craft all necessary materials in order to craft battery (<color=yellow>Fibrous leaves</color> + <color=yellow>Bio fuel</color> + <color=yellow>Battery Core</color>)."",
        ""russian"": ""Ваш скафандр нуждается в энергии, чтобы функционировать. Создайте все необходимые материалы для изготовления батареи (<color=yellow>Волокнистые листья</color> + <color=yellow>Биотопливо</color> + <color=yellow>Ядро батареи</color>)."",
        ""chinese"": ""你的宇航服需要能量才能正常工作。 制作所有这些物品来制作电池 (<color=yellow>纤维叶</color> + <color=yellow>生物燃料</color> + <color=yellow>电芯</color>)."",
        ""slovak"": ""Váš oblek potrebuje energiu na fungovanie. Najprv vyrobte všetky potrebné veci, aby ste mohli zostrojiť batériu. (<color=yellow>Vláknité listy</color> + <color=yellow>Biopalivo</color> + <color=yellow>Jadro batérie</color>).""
      },
      {
        ""identifier"": ""CraftAndUseWaterDesc"",
        ""english"": ""Craft <color=yellow>[Purified Water]</color>. Then go to <color=yellow>[Inventory]</color> and use drag and drop to put it into the <color=yellow>[Water slot]</color>."",
        ""russian"": ""Создайте <color=yellow>[Дистиллированную воду]</color> и перейдите в меню <color=yellow>[Инвентарь]</color>. Перетащите его значок на <color=yellow>[Резервуар для воды]</color>."",
        ""chinese"": ""创建<color=yellow>[蒸馏水]</color>并转到<color=yellow>[库存]</color>菜单。将其图标拖至<color=yellow>[水箱]</color>。"",
        ""slovak"": ""Vytvorte <color=yellow>[Destilovanú vodu]</color> a prejdite do menu <color=yellow>[Inventár]</color>. Potiahnite jej ikonu do <color=yellow>[Zásobníka vody]</color>.""
      },
      {
        ""identifier"": ""BuildBaseDesc"",
        ""english"": ""Build a <color=yellow>Bio fuel Generator</color> in the <color=yellow>[Production]</color> tab. Then go to the <color=yellow>[Base]</color> tab and drag its icon inside the construction grid."",
        ""russian"": ""Создайте <color=yellow>Генератор биотоплива</color> на вкладке <color=yellow>[Производство]</color>. Затем перейдите на вкладку <color=yellow>[Base]</color> и перетащите ее значок внутрь сетки построения."",
        ""chinese"": ""在<color=yellow>[生产]</color>选项卡中构建<color=yellow>生物燃料发电机</color>。然后转到<color=yellow>[基地建设]</color>选项卡并将其图标拖动到构造网格内。"",
        ""slovak"": ""Zostavte <color=yellow>Generátor biopalív</color> na karte <color=yellow>[Produkcia]</color>. Potom prejdite na kartu <color=yellow>[Základňa]</color> a presuňte jej ikonu do konštrukčnej mriežky.""
      },
      {
        ""identifier"": ""BiologySkill"",
        ""english"": ""Biology skill"",
        ""russian"": ""Навык биологии"",
        ""chinese"": ""生物学技能"",
        ""slovak"": ""Biologická zručnosť""
      },
      {
        ""identifier"": ""BiologySkillDesc"",
        ""english"": ""- required for more complex plant or flesh based products - discovers new types and more complex recipes - useful in combat against biological targets"",
        ""russian"": ""- требуется для более сложных продуктов на растительной или мясной основе - открывает новые типы и более сложные рецепты - полезен в борьбе с биологическими целями"",
        ""chinese"": ""- 更复杂的植物或肉类产品需要此技能 - 发现新类型和更复杂的食谱 - 有助于对抗生物目标"",
        ""slovak"": ""- vyžaduje sa pre zložitejšie rastlinné alebo mäsové výrobky - objavuje nové druhy a zložitejšie recepty - užitočné v boji proti biologickým cieľom""
      },
      {
        ""identifier"": ""ChemistrySkill"",
        ""english"": ""Chemistry skill"",
        ""russian"": ""Химический навык"",
        ""chinese"": ""化学技能"",
        ""slovak"": ""Chemické zručnosti""
      },
      {
        ""identifier"": ""ChemistrySkillDesc"",
        ""english"": ""Plants, Liquid and Powders production speed  + 1% per level."",
        ""russian"": ""Скорость производства установок, жидкостей и порошков +1% за уровень."",
        ""chinese"": ""+1% 植物、液体和粉末生产速度"",
        ""slovak"": ""Rýchlosť výroby rastlín, tekutín a práškov + 1 % za každú úroveň.""
      },
      {
        ""identifier"": ""EngineeringSkill"",
        ""english"": ""Engineering skill"",
        ""russian"": ""Инженерное мастерство"",
        ""chinese"": ""工程技能"",
        ""slovak"": ""Inžinierske zručnosti""
      },
      {
        ""identifier"": ""EngineeringSkillDesc"",
        ""english"": ""Mining and construction + 1% per level."",
        ""russian"": ""Горное дело и строительство +1% за уровень."",
        ""chinese"": ""采矿和建筑 + 每级 1%。"",
        ""slovak"": ""Ťažba a stavebníctvo + 1 % za úroveň.""
      },
      {
        ""identifier"": ""AutoUse"",
        ""english"": ""Automatic use"",
        ""russian"": ""Автоматическое использование"",
        ""chinese"": ""自动使用"",
        ""slovak"": ""Automatické použitie""
      },
      {
        ""identifier"": ""AutoUseDesc"",
        ""english"": ""AWhen a given item runs out, another item of the same type is automatically set from the inventory."",
        ""russian"": ""Когда данный предмет заканчивается, из инвентаря автоматически выбирается другой предмет того же типа."",
        ""chinese"": ""当给定的物品用完时，会自动从库存中设置另一件相同类型的物品。"",
        ""slovak"": ""Keď sa daný predmet minie, automaticky sa z inventára nastaví ďalší predmet rovnakého typu.""
      },
      {
        ""identifier"": ""NotEnoughMaterials"",
        ""english"": ""Not enough materials!"",
        ""russian"": ""Недостаточно материалов!"",
        ""chinese"": ""材料不够！"",
        ""slovak"": ""Nedostatok materiálov!""
      },
      {
        ""identifier"": ""AutomateBattery"",
        ""english"": ""Automate battery production"",
        ""russian"": ""Автоматизируйте производство аккумуляторов"",
        ""chinese"": ""实现电池生产自动化"",
        ""slovak"": ""Automatizujte výrobu batérií""
      },
      {
        ""identifier"": ""BuildBase"",
        ""english"": ""Build and place the first building"",
        ""russian"": ""Постройте и разместите свое первое здание"",
        ""chinese"": ""建造并放置第一座建筑物"",
        ""slovak"": ""Postavte a umiestnite svoju prvú budovu""
      },
      {
        ""identifier"": ""CraftAndUseWater"",
        ""english"": ""Craft and use distilled water"",
        ""russian"": ""Создайте и используйте дистиллированную воду"",
        ""chinese"": ""让我们制作并使用蒸馏水!"",
        ""slovak"": ""Pripravte a použite destilovanú vodu""
      },
      {
        ""identifier"": ""CraftBattery"",
        ""english"": ""Craft a battery"",
        ""russian"": ""Создайте батарею"",
        ""chinese"": ""做电池吧！"",
        ""slovak"": ""Vyrobte batériu!""
      },
      {
        ""identifier"": ""IronSheet"",
        ""english"": ""Iron sheet"",
        ""russian"": ""Железный лист"",
        ""chinese"": ""铁皮"",
        ""slovak"": ""Železný plech""
      },
      {
        ""identifier"": ""IronSheetDesc"",
        ""english"": ""Material required for building advanced metal products."",
        ""russian"": ""Материал, необходимый для изготовления современных металлических изделий."",
        ""chinese"": ""这是制造先进金属产品所需的常见材料。"",
        ""slovak"": ""Materiál potrebný na výrobu pokročilých kovových výrobkov.""
      },
      {
        ""identifier"": ""IronRod"",
        ""english"": ""Iron rod"",
        ""russian"": ""Железный стержень"",
        ""chinese"": ""铁棒"",
        ""slovak"": ""Železná tyč""
      },
      {
        ""identifier"": ""IronRodDesc"",
        ""english"": ""Standard component of metallurgical products."",
        ""russian"": ""Стандартный компонент металлургической продукции."",
        ""chinese"": ""铁棒是冶金产品的标准件。"",
        ""slovak"": ""Bežný prvok hutníckych výrobkov.""
      },
      {
        ""identifier"": ""IronTube"",
        ""english"": ""Iron tube"",
        ""russian"": ""Железная трубка"",
        ""chinese"": ""铁管"",
        ""slovak"": ""Železná trubka""
      },
      {
        ""identifier"": ""IronTubeDesc"",
        ""english"": ""Standard building component for handling fluids or gas."",
        ""russian"": ""Стандартный строительный компонент для работы с жидкостями или газом."",
        ""chinese"": ""这是用于处理流体或气体的标准建筑组件。"",
        ""slovak"": ""Štandardný stavebný prvok na manipuláciu s kvapalinami alebo plynmi.""
      },
      {
        ""identifier"": ""Movement"",
        ""english"": ""Movement"",
        ""russian"": ""Движение"",
        ""chinese"": ""移动"",
        ""slovak"": ""Pohyb""
      },
      {
        ""identifier"": ""Weather"",
        ""english"": ""Weather"",
        ""russian"": ""Погода"",
        ""chinese"": ""天气"",
        ""slovak"": ""Počasie""
      },
      {
        ""identifier"": ""Route"",
        ""english"": ""Route"",
        ""russian"": ""Маршрут"",
        ""chinese"": ""路线"",
        ""slovak"": ""Trasa""
      },
      {
        ""identifier"": ""Visibility"",
        ""english"": ""Visibility"",
        ""russian"": ""Видимость"",
        ""chinese"": ""能见度"",
        ""slovak"": ""Viditeľnosť""
      },
      {
        ""identifier"": ""PerceptionArea"",
        ""english"": ""Perception area"",
        ""russian"": ""Область восприятия"",
        ""chinese"": ""感知区"",
        ""slovak"": ""Oblasť vnímania""
      },
      {
        ""identifier"": ""PickupRadius"",
        ""english"": ""Pick-up radius"",
        ""russian"": ""Круг сбора"",
        ""chinese"": ""物品拾取距离"",
        ""slovak"": ""Okruh zberu""
      },
      {
        ""identifier"": ""Movement"",
        ""english"": ""Movement"",
        ""russian"": ""Движение"",
        ""chinese"": ""移动"",
        ""slovak"": ""Pohyb""
      },
      {
        ""identifier"": ""Durability"",
        ""english"": ""Durability:"",
        ""russian"": ""Долговечность:"",
        ""chinese"": ""耐用性:"",
        ""slovak"": ""Trvanlivosť:""
      },
      {
        ""identifier"": ""PhysicalProtection"",
        ""english"": ""Physical protection:"",
        ""russian"": ""Физическая защита:"",
        ""chinese"": ""物理保护:"",
        ""slovak"": ""Fyzická ochrana:""
      },
      {
        ""identifier"": ""FireProtection"",
        ""english"": ""Fire protection:"",
        ""russian"": ""Огнестойкость:"",
        ""chinese"": ""防火性能:"",
        ""slovak"": ""Požiarna odolnosť:""
      },
      {
        ""identifier"": ""ColdProtection"",
        ""english"": ""Cold protection:"",
        ""russian"": ""Холодостойкость:"",
        ""chinese"": ""耐寒性:"",
        ""slovak"": ""Odolnosť proti chladu:""
      },
      {
        ""identifier"": ""GasProtection"",
        ""english"": ""Gas protection:"",
        ""russian"": ""Газостойкость:"",
        ""chinese"": ""耐气体性:"",
        ""slovak"": ""Odolnosť proti plynom:""
      },
      {
        ""identifier"": ""ExplosionProtection"",
        ""english"": ""Explosion protection:"",
        ""russian"": ""Взрывостойкость:"",
        ""chinese"": ""防爆性能:"",
        ""slovak"": ""Odolnosť proti explózií:""
      },
      {
        ""identifier"": ""ShieldPoints"",
        ""english"": ""Shield points:"",
        ""russian"": ""Очки щита:"",
        ""chinese"": ""护盾点数:"",
        ""slovak"": ""Body štítu:""
      },
      {
        ""identifier"": ""HitPoints"",
        ""english"": ""Hit points:"",
        ""russian"": ""Очки жизни:"",
        ""chinese"": ""命值点:"",
        ""slovak"": ""Body zdravia:""
      },
      {
        ""identifier"": ""EnergyCapacity:"",
        ""english"": ""Hit points:"",
        ""russian"": ""Энергетическая мощность:"",
        ""chinese"": ""能量容量:"",
        ""slovak"": ""Energetická kapacita:""
      },
      {
        ""identifier"": ""InventorySlots"",
        ""english"": ""Inventory slots:"",
        ""russian"": ""Слоты инвентаря:"",
        ""chinese"": ""额外的库存槽位:"",
        ""slovak"": ""Miesta v inventári:""
      },
      {
        ""identifier"": ""VisibilityRadius"",
        ""english"": ""Viewable range:"",
        ""russian"": ""Область видимости:"",
        ""chinese"": ""可视范围:"",
        ""slovak"": ""Oblasť viditeľnosti:""
      },
      {
        ""identifier"": ""ExplorationRadius"",
        ""english"": ""Exploration radius:"",
        ""russian"": ""Объем разведки:"",
        ""chinese"": ""探索范围:"",
        ""slovak"": ""Rozsah prieskumu:""
      },
      {
        ""identifier"": ""PickupRadius"",
        ""english"": ""Pickup radius:"",
        ""russian"": ""Объем сбора:"",
        ""chinese"": ""征集范围:"",
        ""slovak"": ""Rozsah zbieru:""
      },
      {
        ""identifier"": ""ProductionSpeed"",
        ""english"": ""Production speed:"",
        ""russian"": ""Скорость производства:"",
        ""chinese"": ""生产速度:"",
        ""slovak"": ""Rýchlosť výroby:""
      },
      {
        ""identifier"": ""MaterialCost"",
        ""english"": ""Material cost:"",
        ""russian"": ""Расход материала:"",
        ""chinese"": ""材料消耗:"",
        ""slovak"": ""Spotreba materiálu:""
      },
      {
        ""identifier"": ""OutcomeRate"",
        ""english"": ""Production quantity:"",
        ""russian"": ""Количество продукции:"",
        ""chinese"": ""生产数量:"",
        ""slovak"": ""Výrobné množstvo:""
      },
      {
        ""identifier"": ""ExecutionFailed"",
        ""english"": ""Execution failed"",
        ""russian"": ""Выполнение не удалось"",
        ""chinese"": ""执行失败"",
        ""slovak"": ""Vykonanie zlyhalo""
      },
      {
        ""identifier"": ""OxygenNotRequired"",
        ""english"": ""You don't need an oxygen tank in this area."",
        ""russian"": ""Вам не нужен кислородный баллон в этой области."",
        ""chinese"": ""在这个区域你不需要氧气瓶。"",
        ""slovak"": ""V tejto oblasti nepotrebujete kyslíkovú nádrž.""
      },
      {
        ""identifier"": ""SplitFailed"",
        ""english"": ""You can't split this item at its current quantity."",
        ""russian"": ""Вы не можете разделить этот товар в его текущем количестве."",
        ""chinese"": ""您无法按当前数量拆分此商品。"",
        ""slovak"": ""Túto vec nemôžete rozdeliť v aktuálnom množstve.""
      },
      {
        ""identifier"": ""FabricatorNotEquipped"",
        ""english"": ""Fabricator is not equipped!"",
        ""russian"": ""Производитель не оборудован!"",
        ""chinese"": ""制造装置未配备！"",
        ""slovak"": ""Výrobník nemáte vo vybavení!""
      },
      {
        ""identifier"": ""FAB-1Desc"",
        ""english"": ""The Fabricator: a handheld marvel, seamlessly transforming raw resources into any item imaginable, from advanced weaponry to essential tools, unlocking limitless possibilities."",
        ""russian"": ""Фабрикатор: портативное чудо, плавно превращающее сырые ресурсы в любые предметы, которые только можно вообразить, от передового оружия до необходимых инструментов, открывая безграничные возможности."",
        ""chinese"": ""制造者：一种手持式奇迹，可以将原始资源无缝地转化为任何可以想象的物品，从先进武器到基本工具，释放无限的可能性。"",
        ""slovak"": ""Výrobník: ručný zázrak, ktorý plynule premieňa surové zdroje na akýkoľvek predmet, ktorý si dokážete predstaviť, od pokročilých zbraní až po základné nástroje, odomykajúci neobmedzené možnosti.""
      },
      {
        ""identifier"": ""Research"",
        ""english"": ""Research"",
        ""russian"": ""Исследовать"",
        ""chinese"": ""研究"",
        ""slovak"": ""Výskum""
      },
      {
        ""identifier"": ""ResearchPoint"",
        ""english"": ""Research point"",
        ""russian"": ""Точка исследования"",
        ""chinese"": ""研究点"",
        ""slovak"": ""Výskumný bod""
      },
      {
        ""identifier"": ""ResearchScienceProjects"",
        ""english"": ""Complete a research 'Science projects'"",
        ""russian"": ""Завершить исследование «Научные проекты»"",
        ""chinese"": ""完成一项研究“科学项目”"",
        ""slovak"": ""Dokončite výskum „Vedecké projekty“""
      },
      {
        ""identifier"": ""ResearchScienceProjectsDesc"",
        ""english"": ""Place a <color=yellow>Research device</color> in your base to generate research points. Then complete <color=yellow>Science Project</color> research."",
        ""russian"": ""Разместите <color=yellow>Исследовательское устройство</color> на своей базе, чтобы получать очки исследования. Затем завершите исследование <color=yellow>Научного проекта</color>."",
        ""chinese"": ""在你的基地放置一个<color=yellow>研究装置</color>来生成研究点。然后完成<color=yellow>科学项目</color>研究。"",
        ""slovak"": ""Umiestnite <color=yellow>Výskumné zariadenie</color> do svojej základne a generujte výskumné body. Potom dokončite výskum <color=yellow>Vedeckého projektu</color>.""
      },
      {
        ""identifier"": ""ScienceProjects"",
        ""english"": ""Science projects"",
        ""russian"": ""Научные проекты"",
        ""chinese"": ""科学项目"",
        ""slovak"": ""Vedecké projekty""
      },
      {
        ""identifier"": ""ScienceProjectsDesc"",
        ""english"": ""Enables the research of multiple categories."",
        ""russian"": ""Позволяет исследовать несколько категорий."",
        ""chinese"": ""该项目允许进行多个类别的研究。"",
        ""slovak"": ""Umožňuje výskum viacerých kategórií.""
      },
      {
        ""identifier"": ""Rewards"",
        ""english"": ""Rewards:"",
        ""russian"": ""Награды:"",
        ""chinese"": ""奖励："",
        ""slovak"": ""Odmeny:""
      },
      {
        ""identifier"": ""Requirements"",
        ""english"": ""Requirements:"",
        ""russian"": ""Требования:"",
        ""chinese"": ""要求："",
        ""slovak"": ""Požiadavky:""
      },
      {
        ""identifier"": ""SteamPower"",
        ""english"": ""Steam power"",
        ""russian"": ""Сила пара"",
        ""chinese"": ""蒸汽动力"",
        ""slovak"": ""Parný pohon""
      },
      {
        ""identifier"": ""SteamPowerDesc"",
        ""english"": ""Unlocks the usage of steam as a resource together with steam powered buildings."",
        ""russian"": ""Открывает возможность использования пара в качестве ресурса вместе со зданиями, работающими на паре."",
        ""chinese"": ""解锁蒸汽作为资源以及蒸汽动力建筑的使用。"",
        ""slovak"": ""Odomkne využitie pary ako zdroja spolu s budovami poháňanými parou.""
      }
    ]
  }
";
    }
}
