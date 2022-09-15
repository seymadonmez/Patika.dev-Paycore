namespace PatikaPayCoreAssignment2.Entity
{
    public class CommonResponse<Entity>
    {
        public CommonResponse()
        {

        }
        public CommonResponse(Entity data)
        {
            Data = data;
        }
        public CommonResponse(string error)
        {
            Error = error;
            Success = false;
        }
        public bool Success { get; set; } = true;
        public string Error { get; set; }
        public Entity Data { get; set; }
    }

}
