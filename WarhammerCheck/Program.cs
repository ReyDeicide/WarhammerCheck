using System.Diagnostics.Metrics;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Weapon
{
    public string Name { get; set; }
    public int A { get; set; }
    public int S { get; set; }
    public int AP { get; set; }
    public int D { get; set; }
    public bool LH { get; set; }
    public int SH { get; set; }
    public bool DW { get; set; }

    public Weapon(string name, int a, int s, int ap, int d, bool lh, int sh, bool dw)
    {
        Name = name;
        A = a;
        S = s;
        AP = ap; D = d;
        LH = lh; SH = sh;
    }
}

class RangeWeapon : Weapon
{
    public int BS { get; set; }
    public bool Heavy { get; set; }

    public RangeWeapon(string name, int a, int bs, int s, int ap, int d, bool lh, int sh, bool dw, bool heavy) : base(name, a, s, ap, d, lh, sh, dw)
    {
        BS = bs;
        Heavy = Heavy;
    }
}

class MeleeWeapon : Weapon
{
    public int WS { get; set; }

    public MeleeWeapon(string name, int a, int ws, int s, int ap, int d, bool lh, int sh, bool dw) : base(name, a, s, ap, d, lh, sh, dw)
    {
        WS = ws;
    }
}

class Unit
{
    public string Fraction { get; set; }
    public string UnitName { get; set; }
    public int Quantity { get; set; }
    public int T { get; set; }
    public int W { get; set; }
    public int Sv { get; set; }
    public int Inv { get; set; }
    public int FNP { get; set; }
    public bool HalveDamage { get; set; }
    public RangeWeapon RangeWeapon { get; set; }
    public MeleeWeapon MeleeWeapon { get; set; }

    public Unit(string fraction, string unitName, int quantity, int t, int w, int sv, int inv, int fnp, bool halveDamage, RangeWeapon rangeWeapon, MeleeWeapon meleeWeapon)
    {
        Fraction = fraction;
        UnitName = unitName;
        Quantity = quantity;
        T = t;
        W = w;
        Sv = sv;
        Inv = inv;
        FNP = fnp;
        HalveDamage = halveDamage;
        RangeWeapon = rangeWeapon;
        MeleeWeapon = meleeWeapon;
    }

    public void PrintUnitDetails()
    {
        Console.WriteLine($"Фракция: {Fraction}");
        Console.WriteLine($"Название отряда: {UnitName}");
        Console.WriteLine($"Количество моделей: {Quantity}");
        Console.WriteLine($"Стойкость: {T}");
        Console.WriteLine($"Количество ран: {W}");
        Console.WriteLine($"Спасбросок: {Sv}+");

        if (Inv != 0) 
        {
            Console.WriteLine($"Инвуль: {Inv}+");
        }

        if (FNP != 0)
        {
            Console.WriteLine($"ФНП: {FNP}+");
        }
        
        if (HalveDamage == true)
        {
            Console.WriteLine("Урон по отряду снижается вдвое");
        }

        Console.WriteLine("[Оружие дальнего боя]");
        Console.WriteLine($"Оружие: {RangeWeapon.Name} Количество атак: {RangeWeapon.A} Точность: {RangeWeapon.BS} Сила: {RangeWeapon.S} Пробитие брони: {RangeWeapon.AP} " +
        $"Урон: {RangeWeapon.D}");
        Console.WriteLine("[Оружие ближнего боя]");
        Console.WriteLine($"Оружие: {MeleeWeapon.Name} Количество атак: {MeleeWeapon.A} Точность: {MeleeWeapon.WS} Сила: {MeleeWeapon.S} Пробитие брони: {MeleeWeapon.AP} " +
        $"Урон: {MeleeWeapon.D}");
        Console.WriteLine("\n");

    }

}

class Program
{
    public static void FireCheck(Unit unit_attacker, Unit unit_defender, int count)
    {
        while (count > 0)
        {
            Random random = new Random();
            int totalAttacks = unit_attacker.RangeWeapon.A * unit_attacker.Quantity;
            int woundsCounter = 0;
            int woundCheck = 0;

            if ((unit_attacker.RangeWeapon.S * 2) <= unit_defender.T)
            {
                woundCheck = 6;
            }

            if ((unit_attacker.RangeWeapon.S * 2)! > unit_defender.T && (unit_attacker.RangeWeapon.S * 2) != unit_defender.T && unit_attacker.RangeWeapon.S < unit_defender.T)
            {
                woundCheck = 5;
            }

            if (unit_attacker.RangeWeapon.S == unit_defender.T)
            {
                woundCheck = 4;
            }

            if (unit_attacker.RangeWeapon.S > unit_defender.T && unit_attacker.RangeWeapon.S != (unit_defender.T * 2) && unit_attacker.RangeWeapon.S! > (unit_defender.T * 2))
            {
                woundCheck = 3;
            }

            if (unit_attacker.RangeWeapon.S >= (unit_defender.T * 2))
            {
                woundCheck = 2;
            }

            while (totalAttacks > 0)
            {
                if (random.Next(1, 6) >= unit_attacker.RangeWeapon.BS)
                {
                    if (random.Next(1, 6) >= woundCheck)
                    {
                        if (random.Next(1, 6) >= (unit_defender.Sv + unit_attacker.RangeWeapon.AP))
                        {
                            woundsCounter += unit_attacker.RangeWeapon.D;
                        }
                    }
                }

                totalAttacks--;
            }

            Console.WriteLine($"Отряд получил {woundsCounter} ран!");
            count--;
        }
    }

    static void FightCheck(Unit unit_attacker, Unit unit_defender, int count)
    {
        while (count > 0)
        {
            Random random = new Random();
            int totalAttacks = unit_attacker.MeleeWeapon.A * unit_attacker.Quantity;
            int woundsCounter = 0;
            int woundCheck = 0;

            if ((unit_attacker.MeleeWeapon.S * 2) <= unit_defender.T)
            {
                woundCheck = 6;
            }

            if ((unit_attacker.MeleeWeapon.S * 2)! > unit_defender.T && (unit_attacker.MeleeWeapon.S * 2) != unit_defender.T && unit_attacker.MeleeWeapon.S < unit_defender.T)
            {
                woundCheck = 5;
            }

            if (unit_attacker.MeleeWeapon.S == unit_defender.T)
            {
                woundCheck = 4;
            }

            if (unit_attacker.MeleeWeapon.S > unit_defender.T && unit_attacker.MeleeWeapon.S != (unit_defender.T * 2) && unit_attacker.MeleeWeapon.S! > (unit_defender.T * 2))
            {
                woundCheck = 3;
            }

            if (unit_attacker.RangeWeapon.S >= (unit_defender.T * 2))
            {
                woundCheck = 2;
            }

            while (totalAttacks > 0)
            {
                if (random.Next(1, 6) >= unit_attacker.MeleeWeapon.WS)
                {
                    if (random.Next(1, 6) >= woundCheck)
                    {
                        if (random.Next(1, 6) >= (unit_defender.Sv + unit_attacker.MeleeWeapon.AP))
                        {
                            woundsCounter += unit_attacker.MeleeWeapon.D;
                        }
                    }
                }

                totalAttacks--;
            }

            Console.WriteLine($"Отряд получил {woundsCounter} ран!");
            count--;
        }
    }

    static void Main()
    {
        //Создание инфантрисквада с лазганами
        RangeWeapon lasgun = new RangeWeapon("Лазган", 2, 4, 3, 0, 1, false, 0, false, false);
        MeleeWeapon guard_combatWeapon = new MeleeWeapon("Импровизированное оружие", 1, 4, 3, 0, 1, false, 0, false);
        Unit infantrySquad = new Unit("Имперская гвардия", "Пехотный отряд", 20, 3, 1, 5, 0, 0, false, lasgun, guard_combatWeapon);

        //Создание тактички с болтерами
        RangeWeapon Boltgun = new RangeWeapon("Болтер", 2, 3, 4, 0, 1, false, 0, false, false);
        MeleeWeapon marine_combatWeapon = new MeleeWeapon("Импровизированное оружие", 1, 3, 4, 0, 1, false, 0, false);
        Unit tacticalSquad = new Unit("Космодесант", "Тактический отряд", 10, 4, 2, 3, 0, 0, false, Boltgun, marine_combatWeapon);

        //Орочьи флешки 10 штук
        RangeWeapon FLESHSHOOTA = new RangeWeapon("Стреляло", 3, 5, 6, 1, 2, false, 0, false, false);
        MeleeWeapon FLESHCHOPPA = new MeleeWeapon("Рубило", 4, 3, 5, 1, 1, false, 0, false);
        Unit FLASHGITZ = new Unit("Орки", "Флешки", 10, 5, 1, 5, 0, 0, false, FLESHSHOOTA, FLESHCHOPPA);

        FireCheck(tacticalSquad, FLASHGITZ, 10);
    }
    
}