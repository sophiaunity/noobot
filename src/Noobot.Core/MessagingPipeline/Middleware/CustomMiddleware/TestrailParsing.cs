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
    internal class TestrailParsing
    {
        public JArray ParseSections(JArray array)
        {
            foreach (JObject arrayObject in array)
            {
                arrayObject.Property("suite_id").Remove();
                arrayObject.Property("parent_id").Remove();
                arrayObject.Property("display_order").Remove();
                arrayObject.Property("depth").Remove();
            }            
            return array;
        }

        public JArray ParseSuites(JArray array)
        {
            foreach (JObject arrayObject in array)
            {
                arrayObject.Property("project_id").Remove();
                arrayObject.Property("is_master").Remove();
                arrayObject.Property("is_baseline").Remove();
                arrayObject.Property("is_completed").Remove();
                arrayObject.Property("completed_on").Remove();
            }
            return array;
        }

        public JObject ParseSuiteID(JObject jObj)
        {
            jObj.Property("id").Remove();
            jObj.Property("project_id").Remove();
            jObj.Property("is_master").Remove();
            jObj.Property("is_baseline").Remove();
            jObj.Property("is_completed").Remove();
            jObj.Property("completed_on").Remove();
            return jObj;
        }

        public JArray ParseProjects(JArray array)
        {
            foreach (JObject arrayObject in array)
            {
                arrayObject.Property("show_announcement").Remove();
                arrayObject.Property("announcement").Remove();
                arrayObject.Property("is_completed").Remove();
                arrayObject.Property("completed_on").Remove();
                arrayObject.Property("suite_mode").Remove();
            }
            return array;
        }

        public JArray ParsePlans(JArray array)
        {
            foreach (JObject arrayObject in array)
            {
                arrayObject.Property("assignedto_id").Remove();
                arrayObject.Property("is_completed").Remove();
                arrayObject.Property("completed_on").Remove();
                arrayObject.Property("blocked_count").Remove();
                arrayObject.Property("retest_count").Remove();
                arrayObject.Property("custom_status1_count").Remove();
                arrayObject.Property("custom_status2_count").Remove();
                arrayObject.Property("custom_status3_count").Remove();
                arrayObject.Property("custom_status4_count").Remove();
                arrayObject.Property("custom_status5_count").Remove();
                arrayObject.Property("custom_status6_count").Remove();
                arrayObject.Property("custom_status7_count").Remove();
                arrayObject.Property("created_on").Remove();
                arrayObject.Property("created_by").Remove();
            }
            return array;
        }

        public JArray ParseRuns(JArray array)
        {
            foreach (JObject arrayObject in array)
            {
                arrayObject.Property("assignedto_id").Remove();
                arrayObject.Property("config").Remove();
                arrayObject.Property("config_ids").Remove();
                arrayObject.Property("completed_on").Remove();
                arrayObject.Property("blocked_count").Remove();
                arrayObject.Property("retest_count").Remove();
                arrayObject.Property("milestone_id").Remove();
                arrayObject.Property("project_id").Remove();
                arrayObject.Property("include_all").Remove();
                arrayObject.Property("custom_status1_count").Remove();
                arrayObject.Property("custom_status2_count").Remove();
                arrayObject.Property("custom_status3_count").Remove();
                arrayObject.Property("custom_status4_count").Remove();
                arrayObject.Property("custom_status5_count").Remove();
                arrayObject.Property("custom_status6_count").Remove();
                arrayObject.Property("custom_status7_count").Remove();
                arrayObject.Property("created_on").Remove();
                arrayObject.Property("created_by").Remove();
            }
            return array;
        }

        public List<Attachment> CreateAttachmentsFromSuites(JArray array)
        {
            List<Attachment> attachments = new List<Attachment>();
            foreach (JObject jObj in array)
            {
                Attachment attach = new Attachment();
                attach.Title = jObj.Property("name").Value.ToString();
                attach.TitleLink = jObj.Property("url").Value.ToString();
                attach.Text = "ID = " + jObj.Property("id").Value.ToString() + "\n" + jObj.Property("description").Value.ToString();
                attachments.Add(attach);
            }
            return attachments;
        }

        public List<Attachment> CreateAttachmentsFromSuiteSearch(JArray array, string searchTerm)
        {
            List<Attachment> attachments = new List<Attachment>();
            foreach (JObject jObj in array)
            {
                Attachment attach = new Attachment();
                if (jObj.Property("name").Value.ToString().ToLower().Contains(searchTerm.ToLower()))
                {
                    attach.Title = jObj.Property("name").Value.ToString();
                    attach.TitleLink = jObj.Property("url").Value.ToString();
                    attach.Text = "ID = " + jObj.Property("id").Value.ToString() + "\n" + jObj.Property("description").Value.ToString();
                    attachments.Add(attach);
                }
            }
            return attachments;
        }

        public List<Attachment> CreateAttachmentsFromSuiteID(JObject jObj)
        {
            List<Attachment> attachments = new List<Attachment>();
            Attachment attach = new Attachment();
            attach.Title = jObj.Property("name").Value.ToString();
            attach.TitleLink = jObj.Property("url").Value.ToString();
            if (!string.IsNullOrEmpty(jObj.Property("description").Value.ToString()))
            {
                attach.Text = jObj.Property("description").Value.ToString();
            }
            else
            {
                attach.Text = "Description = null";
            }
            attachments.Add(attach);
            return attachments;
        }

        public List<Attachment> CreateAttachmentsFromProjects(JArray array)
        {
            List<Attachment> attachments = new List<Attachment>();
            foreach (JObject jObj in array)
            {
                Attachment attach = new Attachment();
                attach.Title = jObj.Property("name").Value.ToString();
                attach.TitleLink = jObj.Property("url").Value.ToString();
                attach.Text = "ID = " + jObj.Property("id").Value.ToString();
                attachments.Add(attach);
            }
            return attachments;
        }

        public List<Attachment> CreateAttachmentsFromSections(JArray array)
        {
            List<Attachment> attachments = new List<Attachment>();
            foreach (JObject jObj in array)
            {
                Attachment attach = new Attachment();
                attach.Title = jObj.Property("name").Value.ToString();
                //attach.TitleLink = jObj.Property("url").Value.ToString();
                attach.Text = "ID = " + jObj.Property("id").Value.ToString() + "\n" + jObj.Property("description").Value.ToString();
                attachments.Add(attach);
            }
            return attachments;
        }
        public List<Attachment> CreateAttachmentsFromPlans(JArray array)
        {
            List<Attachment> attachments = new List<Attachment>();
            foreach (JObject jObj in array)
            {
                Attachment attach = new Attachment();
                attach.Title = jObj.Property("name").Value.ToString();
                attach.TitleLink = jObj.Property("url").Value.ToString();
                attach.Text = "ID = " + jObj.Property("id").Value.ToString() + "\n" + jObj.Property("description").Value.ToString() + "\n" + "Passed: " + jObj.Property("passed_count").Value.ToString() + "\n" + "Failed: " + jObj.Property("failed_count").Value.ToString() + "\n" + "Untested: " + jObj.Property("untested_count").Value.ToString();
                attachments.Add(attach);
            }
            return attachments;
        }

        public List<Attachment> CreateAttachmentsFromRuns(JArray array)
        {
            List<Attachment> attachments = new List<Attachment>();
            foreach (JObject jObj in array)
            {
                Attachment attach = new Attachment();
                attach.Title = jObj.Property("name").Value.ToString();
                attach.TitleLink = jObj.Property("url").Value.ToString();
                attach.Text = "Run ID = " + jObj.Property("id").Value.ToString() + "\n Plan ID = " + jObj.Property("plan_id").Value.ToString() + "\n Suite ID = " + jObj.Property("suite_id").Value.ToString() + "\n" + jObj.Property("description").Value.ToString() + "\n Is Completed: " + jObj.Property("is_completed").Value.ToString() + "\n Passed: " + jObj.Property("passed_count").Value.ToString() + "\n Failed: " + jObj.Property("failed_count").Value.ToString() + "\n Untested: " + jObj.Property("untested_count").Value.ToString();
                attachments.Add(attach);
            }
            return attachments;
        }

        public List<List<Attachment>> SplitList(List<Attachment> listOfAttachments, int nSize)
        {
            var list = new List<List<Attachment>>();

            for (int i=0; i<listOfAttachments.Count; i += nSize)
            {
                list.Add(listOfAttachments.GetRange(i, Math.Min(nSize, listOfAttachments.Count - i)));
            }
            return list;
        }
    }
}