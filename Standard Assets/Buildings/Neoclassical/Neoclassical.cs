using UnityEngine;

using Exception = System.Exception;

public sealed class Neoclassical : Building
{
  // fields
  private const float _component_width_min = 1.5f;
  private const float _component_width_max = 1.75f;
  
  private const float _component_space_min = 2f;
  private const float _component_space_max = 2.25f;

  
  // constructors
  
  /// <summary>
  /// Initializes a new instance of the <see cref="Neoclassical"/> class.
  /// The boundaries of the base of this building must be given in 
  /// clockwise order.
  /// </summary>
  /// <param name='p1'>
  /// A point in space.
  /// </param>
  /// <param name='p2'>
  /// A point in space.
  /// </param>
  /// <param name='p3'>
  /// A point in space.
  /// </param>
  /// <param name='p4'>
  /// A point in space.
  /// </param>
  public Neoclassical (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
  : base(p1, p2, p3, p4, BuildingType.Neoclassical)
  {
    this.FloorHeight = Random.Range(4f, 5f);
    this.FloorNumber = Util.RollDice(new float[] {0.15f, 0.7f, 0.15f});
  }
  
  
  // methods
  
  
  public void ConstructFaceComponents ()
  {
    if (Faces.Count == 0) throw new Exception("There are no faces to construct the components.");
  
    float component_width = Random.Range(_component_width_min, _component_width_max);
    float inbetween_space = Random.Range(_component_space_min, _component_space_max);
  
    foreach (Face face in Faces)
      face.ConstructFaceComponents(component_width, inbetween_space);
  }
}