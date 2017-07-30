using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Upgrade {

    public static List<Upgrade> upgrades = new List<Upgrade>();

    public string title;
    public string description;
    public Action apply;

    public Upgrade(string title, string description, Action action) {
        this.title = title;
        this.description = description;
        this.apply = action;
    }
}
