﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
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
        ""english"": ""Basic resource needed to produce higher level items, such as biofuel."",
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
        ""english"": ""Biofuel"",
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
        ""identifier"": ""BiofuelGenerator"",
        ""english"": ""Biofuel generator"",
        ""russian"": ""Генератор биотоплива"",
        ""chinese"": ""生物燃料发电机"",
        ""slovak"": ""Generátor biopalív""
      },
      {
        ""identifier"": ""BiofuelGeneratorDesc"",
        ""english"": ""Consumes  <color=yellow>Biofuel </color>to generate electricity."",
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
        ""english"": ""Creates <color=yellow>Water</color> from the planet's reserves."",
        ""russian"": ""Создает <color=yellow>Воду</color> из запасов планеты."",
        ""chinese"": ""该设施从这个星球的储备中产生<color=yellow>水</color>。"",
        ""slovak"": ""Ťaží <color=yellow>vodu</color> zo zásob planéty.""
      },
      {
        ""identifier"": ""PlantField"",
        ""english"": ""Plant field"",
        ""russian"": ""Поле растений"",
        ""chinese"": ""植物田"",
        ""slovak"": ""Rastlinné pole""
      },
      {
        ""identifier"": ""PlantFieldDesc"",
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
        ""russian"": ""Восстановить пароль/имя пользователя"",
        ""chinese"": ""恢复密码/用户名"",
        ""slovak"": ""Obnovte heslo / používateľské meno""
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
        ""russian"": ""Продолжать"",
        ""chinese"": ""继续"",
        ""slovak"": ""meno""
      },
      {
        ""identifier"": ""Recover"",
        ""english"": ""Recover"",
        ""russian"": ""Восстановить"",
        ""chinese"": ""恢复"",
        ""slovak"": ""Obnoviť""
        },

      {
        ""identifier"": ""Strength"",
        ""english"": ""Сила"",
        ""russian"": ""Навыки"",
        ""chinese"": ""力量"",
        ""slovak"": ""Sila""
      },
      {
        ""identifier"": ""Perception"",
        ""english"": ""Perception"",
        ""russian"": ""Восприятие"",
        ""chinese"": ""知觉"",
        ""slovak"": ""Vnímanie""
      },
      {
        ""identifier"": ""Intelligence"",
        ""english"": ""Intelligence"",
        ""russian"": ""Интеллект"",
        ""chinese"": ""智力"",
        ""slovak"": ""Inteligencia""
      },
      {
        ""identifier"": ""Agility"",
        ""english"": ""Agility"",
        ""russian"": ""Ловкость"",
        ""chinese"": ""敏捷"",
        ""slovak"": ""Obratnosť""
      },
      {
        ""identifier"": ""Charisma"",
        ""english"": ""Charisma"",
        ""russian"": ""Харизма"",
        ""chinese"": ""魅力"",
        ""slovak"": ""Charizma""
      },
      {
        ""identifier"": ""Willpower"",
        ""english"": ""Willpower"",
        ""russian"": ""Сила воли"",
        ""chinese"": ""意志"",
        ""slovak"": ""Sila vôle""
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
        ""english"": ""Your suit needs energy to function. Craft all necessary materials in order to craft battery (<color=yellow>Fibrous leaves</color> + <color=yellow>Biofuel</color> + <color=yellow>Battery Core</color>)."",
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
        ""english"": ""Build a <color=yellow>Biofuel Generator</color> in the <color=yellow>[Production]</color> tab. Then go to the <color=yellow>[Base]</color> tab and drag its icon inside the construction grid."",
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
      }
    ]
  }
" ;
    }
}
