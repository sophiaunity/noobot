
using System;
using System.Collections.Generic;
using Noobot.Core.MessagingPipeline.Middleware.ValidHandles;
using Noobot.Core.MessagingPipeline.Request;
using Noobot.Core.MessagingPipeline.Response;
using Gurock.TestRail;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Noobot.Core.MessagingPipeline.Middleware.CustomMiddleware
{
    internal class TestrailMiddleware : MiddlewareBase
    {
        public TestrailMiddleware(IMiddleware next) : base(next)
        {
            HandlerMappings = new[]
            {
                new HandlerMapping
                {
                    ValidHandles = new IValidHandle[]
                    {
                        new StartsWithHandle("suites"),
                    },
                    Description = "Lists all suites within the a Testrail project eg suites 2",
                    EvaluatorFunc = SuitesHandler
                },
                new HandlerMapping
                {
                    ValidHandles = new IValidHandle[]
                    {
                        new StartsWithHandle("suite_id"),
                    },
                    Description = "Gets the suite if you know the id. eg suite_id 45",
                    EvaluatorFunc = SuiteIDHandler
                },
                new HandlerMapping
                {
                    ValidHandles = new IValidHandle[]
                    {
                        new ExactMatchHandle("projects"),
                    },
                    Description = "Lists all projects on Testrail",
                    EvaluatorFunc = ListProjectsHandler
                },
                new HandlerMapping
                {
                    ValidHandles = new IValidHandle[]
                    {
                        new StartsWithHandle("sections"),
                    },
                    Description = "Lists all sections within the Unity Testrail project given a suite ID eg sections 2",
                    EvaluatorFunc = SectionsHandler
                },
                new HandlerMapping
                {
                    ValidHandles = new IValidHandle[]
                    {
                        new StartsWithHandle("plans"),
                    },
                    Description = "Lists all plans within a Testrail project given a project ID eg plans 2",
                    EvaluatorFunc = PlansHandler
                },
                new HandlerMapping
                {
                    ValidHandles = new IValidHandle[]
                    {
                        new StartsWithHandle("runs"),
                    },
                    Description = "Lists all runs (that are not a part of a plan) within a Testrail project given a project ID eg runs 2",
                    EvaluatorFunc = RunsHandler
                },
                new HandlerMapping
                {
                    ValidHandles = new IValidHandle[]
                    {
                        new StartsWithHandle("tests"),
                    },
                    Description = "Lists all tests for a test run eg tests 2",
                    EvaluatorFunc = TestsHandler
                }
            };
        }

        private APIClient ConnectToTestrail()
        {
            APIClient client = new APIClient("http://qatestrail.hq.unity3d.com");
            client.User = ""; //TODO - make this able to log in via slack?
            client.Password = ""; //store this in a config file sophiadebug
            return client;
        }

        private IEnumerable<ResponseMessage> SuitesHandler(IncomingMessage message, IValidHandle matchedHandle)
        {
            string searchTerm = message.TargetedText.Substring("suites".Length).Trim();
            yield return message.ReplyDirectlyToUser("sophiadebug search term " + searchTerm);

            if (string.IsNullOrEmpty(searchTerm))
            {
                yield return message.ReplyToChannel("Give me something to search! suites [project_id] eg suites 1");
            }
            else
            {
                //TODO - handle if ID is invalid
                yield return message.IndicateTypingOnChannel();
                APIClient client = ConnectToTestrail();
                string responseFromAPI = "";

                try
                {
                    JArray c = (JArray)client.SendGet($"get_suites/{searchTerm}");
                    responseFromAPI = c.ToString() + "\n I suggest pinning that message so you don't need to request it again!";
                }
                catch (APIException e)
                {
                    responseFromAPI = e.ToString(); // prettify these later
                }
                yield return message.ReplyDirectlyToUser(responseFromAPI);

                //gets the suites in the Unity project

                //Attachment suites = new Attachment();
                //suites.Text = c.ToString();
                //suites.Text = "this is a test";
                //suites.Title = "Suites.txt";

                //yield return message.ReplyToChannel("Here, ", suites);


            }
        }

        private IEnumerable<ResponseMessage> SuiteIDHandler(IncomingMessage message, IValidHandle matchedHandle)
        {
            string searchTerm = message.TargetedText.Substring("suite_id".Length).Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                yield return message.ReplyToChannel("Give me something to search! suite_id [suite_id] eg suite_id 1");
            }
            else
            {
                yield return message.IndicateTypingOnChannel();
                APIClient client = ConnectToTestrail();
                string responseFromAPI = "";

                try
                {
                    JObject c = (JObject)client.SendGet($"get_suite/{searchTerm}");
                    responseFromAPI = c.ToString();
                }
                catch (APIException e)
                {
                    responseFromAPI = e.ToString();
                }
                yield return message.ReplyDirectlyToUser(responseFromAPI);
            }
        }

        private IEnumerable<ResponseMessage> ListProjectsHandler(IncomingMessage message, IValidHandle matchedHandle)
        {
            yield return message.IndicateTypingOnChannel();
            APIClient client = ConnectToTestrail();
            string responseFromAPI = "";

            try
            { 
                JArray c = (JArray)client.SendGet($"get_projects");
                responseFromAPI = c.ToString() + "\n I suggest pinning that message so you don't need to request it again!";
            }
            catch (APIException e)
            {
                responseFromAPI = e.ToString();
            }
            yield return message.ReplyDirectlyToUser(responseFromAPI);
        }

        private IEnumerable<ResponseMessage> SectionsHandler(IncomingMessage message, IValidHandle matchedHandle)
        {
            string searchTerm = message.TargetedText.Substring("sections".Length).Trim();
            yield return message.ReplyDirectlyToUser("sophiadebug search term " + searchTerm);

            if (string.IsNullOrEmpty(searchTerm))
            {
                yield return message.ReplyToChannel("Give me something to search! Needs to be a suite_id within the Unity project. sections [suite_id] eg sections 1");
            }
            else
            {
                yield return message.IndicateTypingOnChannel();
                APIClient client = ConnectToTestrail();
                string responseFromAPI = "";

                try
                {
                    JArray c = (JArray)client.SendGet($"get_sections/2&suite_id={searchTerm}"); //need to get IDs first
                    responseFromAPI = c.ToString();
                }
                catch (APIException e)
                {
                    responseFromAPI = e.ToString();
                }
                yield return message.ReplyDirectlyToUser(responseFromAPI);
            }
        }

        private IEnumerable<ResponseMessage> PlansHandler(IncomingMessage message, IValidHandle matchedHandle)
        {
            string searchTerm = message.TargetedText.Substring("plans".Length).Trim();
            yield return message.ReplyDirectlyToUser("sophiadebug search term " + searchTerm);

            if (string.IsNullOrEmpty(searchTerm))
            {
                yield return message.ReplyToChannel("Give me something to search! plans [project_id] eg plans 1");
            }
            else
            {
                yield return message.IndicateTypingOnChannel();
                APIClient client = ConnectToTestrail();
                string responseFromAPI = "";

                try
                {
                    JArray c = (JArray)client.SendGet($"get_plans/{searchTerm}");
                    responseFromAPI = c.ToString();
                }
                catch (APIException e)
                {
                    responseFromAPI = e.ToString();
                }
                yield return message.ReplyDirectlyToUser(responseFromAPI);
            }
        }

        private IEnumerable<ResponseMessage> RunsHandler(IncomingMessage message, IValidHandle matchedHandle)
        {
            string searchTerm = message.TargetedText.Substring("runs".Length).Trim();
            yield return message.ReplyDirectlyToUser("sophiadebug search term " + searchTerm);

            if (string.IsNullOrEmpty(searchTerm))
            {
                yield return message.ReplyToChannel("Give me something to search! runs [project_id] eg runs 1");
            }
            else
            {
                yield return message.IndicateTypingOnChannel();
                APIClient client = ConnectToTestrail();
                string responseFromAPI = "";

                try
                {
                    JArray c = (JArray)client.SendGet($"get_runs/{searchTerm}");
                    responseFromAPI = c.ToString();
                }
                catch (APIException e)
                {
                    responseFromAPI = e.ToString();
                }
                yield return message.ReplyDirectlyToUser(responseFromAPI);
            }
        }

        private IEnumerable<ResponseMessage> TestsHandler(IncomingMessage message, IValidHandle matchedHandle)
        {
            string searchTerm = message.TargetedText.Substring("tests".Length).Trim();
            yield return message.ReplyDirectlyToUser("sophiadebug search term " + searchTerm);

            if (string.IsNullOrEmpty(searchTerm))
            {
                yield return message.ReplyToChannel("Give me something to search! tests [test_id] eg tests 1");
            }
            else
            {
                yield return message.IndicateTypingOnChannel();
                APIClient client = ConnectToTestrail();
                string responseFromAPI = "";

                try
                {
                    JArray c = (JArray)client.SendGet($"get_tests/{searchTerm}");
                    responseFromAPI = c.ToString();
                }
                catch (APIException e)
                {
                    responseFromAPI = e.ToString();
                }
                yield return message.ReplyDirectlyToUser(responseFromAPI);
            }
        }
    }
}