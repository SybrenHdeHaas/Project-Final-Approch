using GXPEngine;

public class DetectionInfo
{
    public bool intersected;
    public GameObject theObject;

    public DetectionInfo()
    {
    }

    public void UpdateInfo(bool  intersected, GameObject theObject)
    {
        this.intersected = intersected;
        this.theObject = theObject;
    }

}
