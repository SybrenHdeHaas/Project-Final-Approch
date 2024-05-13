using GXPEngine; // For GameObject

//track data about a collision of an object
public class CollisionInfo
{
    public readonly Vec2 normal; //the collision normal
    public readonly GameObject other; //the other object that's collided with
    public readonly float timeOfImpact;
    public readonly float AABBDirection;

    //1 -> up
    //2 -> down
    //3 -> left
    //4 -> right

    public CollisionInfo(Vec2 pNormal, GameObject pOther, float pTimeOfImpact, int aABBDirection=-1)
    {
        normal = pNormal;
        other = pOther;
        timeOfImpact = pTimeOfImpact;
        AABBDirection = aABBDirection;
    }
}
