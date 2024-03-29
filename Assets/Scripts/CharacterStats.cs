﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterStats {

    public string name = "Name";
    protected int _level = 1;

    protected float _xp = 1;
    protected float _xpForCurrentLevel = 1;
    public float xpForNextLevel = 1;
    protected float _maxHP = 10;
    protected float _attack = 2;
    protected float _defense = 0;
    public float currentHP = 0;
    public float statPoints = 0;

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

    public float maxHP
    {
        get { return _maxHP; }
        set { _maxHP = value; }
    }


    public float xpForCurrentLevel
    {
        get { return _xpForCurrentLevel; }
    }


    public float xp
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

    public float attack
    { get { return _attack;}
        set { _attack = value; }
    }

    public float defense
    {
        get { return _defense; }
        set
        {
            _defense = value;
        }
    }

    // enemy HP = newlevel
    public void setLevelandStats(int newlevel)
    {
        _level = newlevel;
        _maxHP =  newlevel;
        _xp = _maxHP;

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
                _defense += 0.01f * numPoints;
            }
            statPoints -= numPoints;


        }
    }

}



