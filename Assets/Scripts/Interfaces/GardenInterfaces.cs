using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iSelectable
{
    void Select();
    string GetName();
    Sprite GetSprite();
    //void Deselect();
}

public interface iSellable
{
    bool IsSellable();
    void OnSell();
    int GetSellPrice();
}

public interface iRequirements
{
    //bool CheckRequirements();
    //void ReportRequirements();
    bool ReportRequirements();
    List<AnimalChecklist> GetRequirements();
}

public interface iPickupable
{
    //void Pickup();
    bool IsPickupable();
    //void Drop();
}

public interface iDirectable
{
    bool IsDirectable();
}

public interface iEdible
{
    bool IsEdible();
    //void GetEaten();
}

//interface for the first time growing an object or taming an animal
public interface iFirstTimeTame
{
    //bool property for first time growing an object or taming an animal
    bool FirstTimeTame { get; set; }
    float FirstTimeTameExperience { get; }
}

public interface iFirstTimeSeen
{
    bool FirstTimeSeen { get; set; }
    float FirstTimeSeenExperience { get; }
}

public interface iTurnOnAndOffAble
{
    bool IsOn { get; }
    void TurnOn();
    void TurnOff();
}

public interface iWaterable
{
    bool IsWaterable();
    void Water(float waterAmount);
}

public interface iEmoji
{
    GifPlayer _GifPlayer { get; }
}