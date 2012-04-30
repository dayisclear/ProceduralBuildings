﻿using UnityEngine;
using System.Collections.Generic;
using Thesis.Base;

namespace Thesis {

public sealed class NeoManager
{
  private static readonly NeoManager _instance = new NeoManager();
  public static NeoManager Instance
  {
    get { return _instance; }
  }

  public List<Neoclassical> neo = new List<Neoclassical>();

  private NeoManager () { }

  public void Init ()
  {
    CreateBalconyTextures();
  }

  public void CreateNeoclassical (BuildMode mode)
  {
    switch (mode)
    {      
      case BuildMode.Many:
        for (int i = 0; i < 25; ++i)
        {
          float x_mod = i * 30f;
          for (int j = 0; j < 25; ++j)
          {
            float z_mod = j * 15f;
            int dice = Util.RollDice(new float[] { 0.25f, 0.25f, 0.25f, 0.25f });
            switch (dice)
            {
              case 1:
                Build(new Vector3(x_mod + 9f, 0f, z_mod + 3.5f),
                      new Vector3(x_mod + 9f, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod + 3.5f));
                break;

              case 2:
                Build(new Vector3(x_mod + 11f, 0f, z_mod + 4f),
                      new Vector3(x_mod + 11f, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod + 4f));
                break;

              case 3:
                Build(new Vector3(x_mod + 15f, 0f, z_mod + 6f),
                      new Vector3(x_mod + 15f, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod + 6f));
                break;

              case 4:
                Build(new Vector3(x_mod + 19f, 0f, z_mod + 8f),
                      new Vector3(x_mod + 19f, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod),
                      new Vector3(x_mod, 0f, z_mod + 8f));
                break;
            }            
          }
        }
        break;

      case BuildMode.Two:
        Build(new Vector3(8f, 0f, 4f),
              new Vector3(8f, 0f, 0f),
              new Vector3(0f, 0f, 0f),
              new Vector3(0f, 0f, 4f));
        break;

      case BuildMode.Three:
        Build(new Vector3(11f, 0f, 4f),
              new Vector3(11f, 0f, 0f),
              new Vector3(0f, 0f, 0f),
              new Vector3(0f, 0f, 4f));
        break;

      case BuildMode.Four:
        Build(new Vector3(15f, 0f, 6f),
              new Vector3(15f, 0f, 0f),
              new Vector3(0f, 0f, 0f),
              new Vector3(0f, 0f, 6f));
        break;

      case BuildMode.Five:
        Build(new Vector3(19f, 0f, 8f),
              new Vector3(19f, 0f, 0f),
              new Vector3(0f, 0f, 0f),
              new Vector3(0f, 0f, 8f));
        break;
    }
  }

  public void DestroyBuildings ()
  {
    foreach (Neoclassical n in neo)
      GameObject.Destroy(n.gameObject);
    neo.Clear();
  }

  private void Build (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
  {
    var n = new Neoclassical(p1, p2, p3, p4);
    n.buildingMesh.FindVertices();
    n.buildingMesh.FindTriangles();
    n.buildingMesh.Draw();
    n.CombineSubmeshes();
    n.gameObject.SetActiveRecursively(true);
    neo.Add(n);
  }

  private void CreateBalconyTextures ()
  {
    var tex = new ProceduralTexture();
    tex.content = new Texture2D(1024, 512);
    tex.Clear();

    var ratio = 2f;
    var outBorderSize = 0.015f;
    var inBorderSize = 0.01f;
    var spaceBetweenBorders = 0.06f;

    int vOutBorderWidth    = Mathf.FloorToInt(tex.content.width * outBorderSize);
    int topOutBorderWidth  = Mathf.FloorToInt(tex.content.height * outBorderSize * 2 * ratio);
    int vInBorderWidth     = Mathf.FloorToInt(tex.content.width * inBorderSize);
    int hInBorderWidth     = Mathf.FloorToInt(tex.content.height * inBorderSize * ratio);
    int vInBorderOffset    = Mathf.FloorToInt(tex.content.width * spaceBetweenBorders) +
                                              vOutBorderWidth;
    int botInBorderOffset  = Mathf.FloorToInt(tex.content.height * spaceBetweenBorders * ratio);

    var halfWidth = tex.content.width >> 1;

    int topOutBorderY    = tex.content.height - (topOutBorderWidth >> 1);
    int botInBorderY     = botInBorderOffset + (hInBorderWidth >> 1);
    int topInBorderY     = tex.content.height - botInBorderY - topOutBorderWidth;
    int leftOutBorderX   = (vOutBorderWidth >> 1) - 1;
    int rightOutBorderX  = tex.content.width - leftOutBorderX;
    int leftInBorderX    = vInBorderOffset + (vInBorderWidth >> 1);
    int rightInBorderX   = tex.content.width - leftInBorderX;

    // top out border
    tex.lines.Add(new TextureLine(0, topOutBorderY,
                                  tex.content.width, topOutBorderY,
                                  Color.black, topOutBorderWidth));
    // bot in border
    tex.lines.Add(new TextureLine(0, botInBorderY,
                                  tex.content.width, botInBorderY,
                                  Color.black, hInBorderWidth));
    // top in border
    tex.lines.Add(new TextureLine(0, topInBorderY,
                                  tex.content.width, topInBorderY,
                                  Color.black, hInBorderWidth));
    // left out border
    tex.lines.Add(new TextureLine(leftOutBorderX, 0,
                                  leftOutBorderX, tex.content.height,
                                  Color.black, vOutBorderWidth));
    // right out border
    tex.lines.Add(new TextureLine(rightOutBorderX, 0,
                                  rightOutBorderX, tex.content.height,
                                  Color.black, vOutBorderWidth));
    // left in border
    tex.lines.Add(new TextureLine(leftInBorderX, 0,
                                  leftInBorderX, tex.content.width,
                                  Color.black, vInBorderWidth));
    // right in border
    tex.lines.Add(new TextureLine(rightInBorderX, 0,
                                  rightInBorderX, tex.content.width,
                                  Color.black, vInBorderWidth));
    // middle border
    tex.lines.Add(new TextureLine(halfWidth, 0,
                                  halfWidth, tex.content.height,
                                  Color.black, vInBorderWidth));
    // left inner box
    tex.lines.Add(new TextureLine(leftInBorderX, botInBorderY + 2,
                                  halfWidth + 1, topInBorderY,
                                  Color.black, vInBorderWidth));

    tex.lines.Add(new TextureLine(leftInBorderX, topInBorderY - 1,
                                  halfWidth + 1, botInBorderY,
                                  Color.black, vInBorderWidth));
    // right inner box
    tex.lines.Add(new TextureLine(halfWidth + 1, botInBorderY,
                                  rightInBorderX, topInBorderY,
                                  Color.black, vInBorderWidth));

    tex.lines.Add(new TextureLine(halfWidth + 1, topInBorderY,
                                  rightInBorderX, botInBorderY,
                                  Color.black, vInBorderWidth));

    tex.Draw();

    TextureManager.Instance.Add("tex_neo_balcony", tex);
  }
}

} // namespace Thesis