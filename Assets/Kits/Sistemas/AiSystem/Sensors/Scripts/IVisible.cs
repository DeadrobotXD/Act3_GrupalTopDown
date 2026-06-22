using UnityEngine;

public interface Ivisible
{
    public enum Side { Friend, Neutral, Enemy };
    public Side GetSide();
    public Transform GetTransform();

}
