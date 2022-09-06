using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStats {

    public string name = "Name";
    protected int _level = 1;

    protected int _xp = 1;
    protected int _xpForCurrentLevel = 1;
    public int xpForNextLevel = 1;
    protected int _maxHP = 10;
    protected int _attack = 2;
    protected double _defense = 0;
    public int currentHP = 0;
    public int statPoints = 0;

    public int speed
    {
        get
        {
            return level;
        }
    }

    //properties
    public virtual int level
    {
        get { return _level; }
        set
        {
            xp = level;
        }
    }

    public int maxHP
    {
        get { return _maxHP; }
        set { _maxHP = value; }
    }


    public int xpForCurrentLevel
    {
        get { return _xpForCurrentLevel; }
    }


    public int xp
    {
        get { return _xp; }
        set
        {
            _xp = value;
            while (_xp > xpForNextLevel)
            {
                _level++;
                statPoints += 1;
                _xpForCurrentLevel = xpForNextLevel;
                xpForNextLevel += 1;
            }
        }
    }

    public int attack
    { get { return _attack;}
        set { _attack = value; }
    }

    public double defense
    {
        get { return _defense; }
        set
        {
            _defense = value;
        }
    }

    public void setLevelandStats(int newlevel)
    {
        _level = newlevel;
        _maxHP = newlevel * 10;
        _attack = (newlevel * 1)+1;
        _defense = newlevel * 1;

    }


    public void spendStatPointsOnHP(int numPoints)
    {
        if (numPoints <= statPoints)
        {
            _maxHP += 10 * numPoints;
            statPoints -= numPoints;
        }
    }
    public void spendStatPointsOnAttack(int numPoints)
    {
        if (numPoints <= statPoints)
        {
            _attack += 1 * numPoints;
            statPoints -= numPoints;
        }
    }
    public void spendStatPointsOnDefense(int numPoints)
    {
        if (numPoints <= statPoints)
        {
            if (_defense == 0)
                _defense = 1;
            else
            {
                _defense += 0.01 * numPoints;
            }
            statPoints -= numPoints;


        }
    }

}


public class CharacterStatsSpeedCompare : IComparer<CharacterBehavior>
{
    public int Compare(CharacterBehavior x, CharacterBehavior y)
    {
        return x.stats.speed - y.stats.speed;
    }
}

