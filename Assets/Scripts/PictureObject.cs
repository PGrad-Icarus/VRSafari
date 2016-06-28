using UnityEngine;
using System.Collections;
using SQLite4Unity3d;

public class PictureObject {
	[PrimaryKey]
	public string name { get; set; }
}