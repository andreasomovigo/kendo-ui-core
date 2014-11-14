﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Kendo.Models
{
    public class AspNetMvcDescription : IFrameworkDescription
    {
        private static readonly String[] Patterns = new[] { "~/src/{0}/views/aspx/{1}/{2}.aspx", "~/src/{0}/views/razor/{1}/{2}.cshtml" };

        public string Name
        {
            get
            {
                return "ASP.NET MVC";
            }
        }

        public IEnumerable<ExampleFile> GetFiles(HttpServerUtilityBase server, string example, string section)
        {
            foreach (var pattern in Patterns)
            {
                yield return new ExampleFile
                {
                    Name = example + Path.GetExtension(pattern),
                    Url = String.Format(pattern, "aspnetmvc", section, example)
                };
            }

            var path = server.MapPath("~/src/aspnetmvc/controllers/");

            if (Directory.Exists(path))
            {
                var sections = Directory.GetDirectories(path);

                var directory = sections.FirstOrDefault(s =>
                {
                    var dir = Path.GetFileName(s.ToLower().Replace("_", "-"));

                    return dir.Equals(section) || dir.EndsWith(section + "s");
                });

                if (directory != null)
                {
                    var controllers = Directory.GetFiles(directory);

                    var controller = controllers.FirstOrDefault(c => Path.GetFileName(c).ToLower().Replace("_", "-") == example + "controller.cs");

                    if (controller != null)
                    {
                        yield return new ExampleFile
                        {
                            Name = Path.GetFileName(controller),
                            Url = "~/src/aspnetmvc/controllers/" + Path.GetFileName(directory) + "/" + Path.GetFileName(controller)
                        };

                        var source = File.ReadAllText(controller);

                        if (source.Contains("productService"))
                        {
                            yield return new ExampleFile
                            {
                                Name = "ProductService.cs",
                                Url = "~/src/aspnetmvc/models/ProductService.cs"
                            };
                        }

                        if (source.Contains("taskService"))
                        {
                            yield return new ExampleFile
                            {
                                Name = "SchedulerTaskService.cs",
                                Url = "~/src/aspnetmvc/models/Scheduler/SchedulerTaskService.cs"
                            };
                        }

                        if (source.Contains("meetingService"))
                        {
                            yield return new ExampleFile
                            {
                                Name = "SchedulerMeetingService.cs",
                                Url = "~/src/aspnetmvc/models/Scheduler/SchedulerMeetingService.cs"
                            };
                        }

                        if (source.Contains("TreeListController"))
                        {
                            yield return new ExampleFile
                            {
                                Name = "EmployeeDirectoryController.cs",
                                Url = "~/src/aspnetmvc/controllers/TreeList/EmployeeDirectoryController.cs"
                            };
                        }
                    }
                }
            }
        }
    }
}
