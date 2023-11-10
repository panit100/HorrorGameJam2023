public class MainObjectiveData{
    public string ObjectiveName;
    public string ObjectiveCode;
    public string ObjectiveType;
    public string NextObjectiveCode;
    public string Sender;
    public string SenderColor;
    public string TimeColor;
    public string MassageColor;
    public string LogMessage;
    public string PuzzleCode;
    
    public ObjectiveType GetObjectiveType()
    {
        switch (ObjectiveType)
        {
            case "ObjectiveType.TakePhoto":
                return global::ObjectiveType.TakePhoto;
            case "ObjectiveType.ReachPosition":
                return global::ObjectiveType.ReachPosition;
            case "ObjectiveType.ScanObject":
                return global::ObjectiveType.ScanObject;
            case "ObjectiveType.Puzzle":
                return global::ObjectiveType.Puzzle;
            default:
                return global::ObjectiveType.TakePhoto;
        }
    }
}
