using UnityEngine;
using System.Collections;
using SQLite4Unity3d;
public class PictureGroup {
	[PrimaryKey]
	public string name { get; set; }
	public int multiplier { get; set; }
}
