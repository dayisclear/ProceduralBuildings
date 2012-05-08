using UnityEngine;

namespace Thesis {

public sealed class NeoBuildingMesh : Base.BuildingMesh
{
  /*************** FIELDS ***************/

  private const float _componentWidthMin = 1.1f;
  private const float _componentWidthMax = 1.25f;
  private const float _componentSpaceMin = 2f;
  private const float _componentSpaceMax = 2.25f;

  public float windowHeight;

  public float doorHeight;

  public float balconyHeight;

  public float balconyFloorHeight;

  public float balconyFloorWidth;

  public float balconyFloorDepth;

  /*************** CONSTRUCTORS ***************/
  
  /// <summary>
  /// Initializes a new instance of the <see cref="NeoBuildingMesh"/> class.
  /// The boundaries of the base of this building must be given in 
  /// clockwise order.
  /// </summary>
  public NeoBuildingMesh (Neoclassical parent, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    : base(parent)
  {
    name = "neo_building_mesh";
    material = MaterialManager.Instance.Get("Building");

    parent.AddCombinable(material.name, this);

    floorHeight = Random.Range(4f, 4.25f);
    floorCount = Util.RollDice(new float[] {0.15f, 0.7f, 0.15f});

    FindMeshOrigin(p1, p3, p2, p4);

    boundaries = new Vector3[8];
    boundaries[0] = p1 - meshOrigin;
    boundaries[1] = p2 - meshOrigin;
    boundaries[2] = p3 - meshOrigin;
    boundaries[3] = p4 - meshOrigin;

    for (int i = 0; i < 4; ++i)
      boundaries[i + 4] = new Vector3(boundaries[i].x,
                                      boundaries[i].y + height,
                                      boundaries[i].z);

    ConstructFaces();
    ConstructFaceComponents();
  }

  /*************** METHODS ***************/

  public override void ConstructFaces ()
  {
    faces.Add(new NeoFace(this, boundaries[0], boundaries[1]));
    faces.Add(new NeoFace(this, boundaries[1], boundaries[2]));
    faces.Add(new NeoFace(this, boundaries[2], boundaries[3]));
    faces.Add(new NeoFace(this, boundaries[3], boundaries[0]));

    SortFaces();
  }
  
  public void ConstructFaceComponents ()
  {
    windowHeight = Random.Range(1.8f, 2f);
    doorHeight = Random.Range(2.8f, 3f);

    balconyHeight = windowHeight / 2 + floorHeight / 2.5f;
    balconyFloorHeight = 0.2f;
    balconyFloorDepth = 1f;
    balconyFloorWidth = 0.6f;

    float component_width = Random.Range(_componentWidthMin, _componentWidthMax);
    float inbetween_space = Random.Range(_componentSpaceMin, _componentSpaceMax);

    foreach (Base.Face face in faces)
      face.ConstructFaceComponents(component_width, inbetween_space);
  }
}

} // namespace Thesis