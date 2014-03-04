using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using NUnit.Framework;
using PerceptiveMCAPI;
using PerceptiveMCAPI.Methods;
using PerceptiveMCAPI.Types;

namespace MailChimpMgr
{
    [TestFixture]
    public class MailChimpAPITest
    {
        private MailChimpAPI mailChimpApi;
        private bool _listSubscribe;
        private string _apiKey;
        private string _listId;

        public MailChimpAPITest()
        {
            SetUp();
        }
        [SetUp]
        protected void SetUp()
        {
            mailChimpApi = new MailChimpAPI();
            _apiKey = "3830abbd807840b9aec54e8d1599862a-us4";
            _listId = "0d59c0d08d";
        }
        [Test]
        public void ListSubscribe()
        {
            var subscriber = new Table.MailChimpContact();
            subscriber.Email = "cnpp@gmail.com";
            subscriber.FirstName = "Jac1";
            subscriber.LastName = "Zhang1";
            Assert.AreEqual(true, mailChimpApi.ListSubscribe(_apiKey, _listId, subscriber));
        }
        [Test]
        public void tListUnSubscribe()
        {
            var subscriber = new Table.MailChimpContact();
            subscriber.Email = "cnpp@gmail.com";
            Assert.AreEqual(true, mailChimpApi.ListUnsubscribe(_apiKey, _listId, subscriber));
        }
        [Test]
        public void ListBatchSubscribe()
        {
            var subscribers = new List<Table.MailChimpContact>
                                  {
                                      new Table.MailChimpContact
                                          {
                                              Email = "cnpp1@gmail.com",
                                              FirstName = "Jac11",
                                              LastName = "Zhang11",
                                          },      new Table.MailChimpContact
                                          {
                                              Email = "cnpp2@gmail.com",
                                              FirstName = "Jac12",
                                              LastName = "Zhang12",
                                          },      new Table.MailChimpContact
                                          {
                                              Email = "cnpp3@gmail.com",
                                              FirstName = "Jac13",
                                              LastName = "Zhang13"
                                          },      new Table.MailChimpContact
                                          {
                                              Email = "cnpp4@gmail.com",
                                              FirstName = "Jac14",
                                              LastName = "Zhang14"

                                          }
                                  };


            Assert.AreEqual(true, mailChimpApi.ListBatchSubscribe(_apiKey, _listId, subscribers));
        }
        [Test]
        public void ListBatchUnsubscribe()
        {
            var subscribers = new List<Table.MailChimpContact>
                                  {
                                      new Table.MailChimpContact
                                          {
                                              Email = "cnpp1@gmail.com",
                                              FirstName = "Jac11",
                                              LastName = "Zhang11",
                                          },      new Table.MailChimpContact
                                          {
                                              Email = "cnpp2@gmail.com",
                                              FirstName = "Jac12",
                                              LastName = "Zhang12",
                                          },      new Table.MailChimpContact
                                          {
                                              Email = "cnpp3@gmail.com",
                                              FirstName = "Jac13",
                                              LastName = "Zhang13"
                                          },      new Table.MailChimpContact
                                          {
                                              Email = "cnpp4@gmail.com",
                                              FirstName = "Jac14",
                                              LastName = "Zhang14"

                                          }
                                  };
            Assert.AreEqual(true, mailChimpApi.ListBatchUnsubscribe(_apiKey, _listId, subscribers));
        }
        [Test]
        public void GetMailChimpLists()
        {
            var result = mailChimpApi.GetMailChimpLists(_apiKey);
            Assert.AreEqual(true, result.Count > 0);
            foreach (var listsResultse in result)
            {
                Console.WriteLine(listsResultse.id);
            }
        }

        [Test]
        public void GetMailChimpCampaigns()
        {
            var result = mailChimpApi.GetMailChimpCampaigns(_apiKey);
            //var result = mailChimpApi.GetMailChimpCampaigns(_apiKey, mailChimpListId: "f15a2d42fb");
            Assert.AreEqual(true, result.Count > 0);
            foreach (var listsResultse in result)
            {
                Console.WriteLine(listsResultse.title);
                //Console.WriteLine(listsResultse.subject);
                //Console.WriteLine(listsResultse.id);
                //Console.WriteLine(listsResultse.list_id);
            }
        }
        [Test]
        public void GetMailChimpCampaignsWithListId()
        {
            //var result = mailChimpApi.GetMailChimpCampaigns(_apiKey);
            var result = mailChimpApi.GetMailChimpCampaigns(_apiKey, mailChimpListId: "f15a2d42fb");
            //var result = mailChimpApi.GetMailChimpCampaigns(_apiKey);
            Assert.AreEqual(true, result.Count > 0);
            foreach (var listsResultse in result)
            {
                Console.WriteLine(listsResultse.subject);
                Console.WriteLine(listsResultse.list_id);
            }
        }
        [Test]
        public void GetMailChimpCampaignMembers()
        {
            //var result = mailChimpApi.GetMailChimpCampaignMembers(_apiKey);
            var result = mailChimpApi.GetMailChimpCampaignMembers(_apiKey, "efe0812168");
            //var result = mailChimpApi.GetMailChimpCampaignMembers("efe0812168");
            Assert.AreEqual(true, result.Count > 0);
            foreach (var listsResultse in result)
            {
                Console.WriteLine(listsResultse.email);
            }

        } 
        [Test]
        public void GetCampaignBounceMessages()
        {
            //var result = mailChimpApi.GetMailChimpCampaignMembers(_apiKey);
            var result = mailChimpApi.GetCampaignBounceMessages(_apiKey, "efe0812168",new DateTime());
            //var result = mailChimpApi.GetMailChimpCampaignMembers("efe0812168");
            Assert.AreEqual(true, result.Count > 0);
            foreach (var listsResultse in result)
            {
                Console.WriteLine(listsResultse.email);
            }
        }
    }
}
