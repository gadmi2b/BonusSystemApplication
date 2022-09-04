////--------------------------------------------------------------------------------------------------------
//using System;
//using System.Collections.Generic;
//using System.Web.Script.Serialization;
//using System.Web.Mvc;

//[HttpPost]
//public JsonResult YourMethodName(string inputData)//JSON should contain key, action, otherThing
//{
//    JsonResult RetVal = new JsonResult();  //We will use this to pass data back to the client

//  try {
//    var JSONObj = new JavaScriptSerializer().Deserialize < Dictionary < string, string>> (inputData);
//        string RequestKey = JSONObj["key"];
//        string RequestAction = JSONObj["action"];
//        string RequestOtherThing = JSONObj["otherThing"];

//    //Use your request information to build your array
//    //You didn't specify what kind of array, but it works the same regardless.
//    int[] ResponseArray = new int[10];

//    //populate array here

//    //Write out the response
//    RetVal = Json(new
//      {
//        Status = "OK",
//        Message = "Response Added",
//        MyArray = ResponseArray
//      });
//  }
//  catch (Exception ex)
//  {
//    //Response if there was an error
//    RetVal = Json(new
//      {
//        Status = "ERROR",
//        Message = ex.ToString(),
//        MyArray = new int[0]
//      });
//  }
//  return RetVal;
//}


