﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartProject.cs" company="The CruiseControl.NET Team">
//   Copyright (C) 2011 by The CruiseControl.NET Team
// 
//   Permission is hereby granted, free of charge, to any person obtaining a copy
//   of this software and associated documentation files (the "Software"), to deal
//   in the Software without restriction, including without limitation the rights
//   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//   copies of the Software, and to permit persons to whom the Software is
//   furnished to do so, subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in
//   all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//   THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ThoughtWorks.CruiseControl.PowerShell.Cmdlets
{
    using System.Management.Automation;
    using ThoughtWorks.CruiseControl.Remote;

    /// <summary>
    /// A cmdlet for starting a project.
    /// </summary>
    [Cmdlet("Start", "Project", DefaultParameterSetName = "PathSet", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
    public class StartProject
        : ProjectCmdlet
    {
        #region Protected methods
        #region ProcessRecord()
        /// <summary>
        /// Processes a record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var projects = this.GetProjects();
            if (projects.Count == 0)
            {
                return;
            }

            foreach (var project in projects)
            {
                if (!this.ShouldProcess(project.Name, "Start a project"))
                {
                    return;
                }

                try
                {
                    project.Start();
                    this.WriteObject(project.Refresh());
                }
                catch (CommunicationsException error)
                {
                    var record = new ErrorRecord(
                        error,
                        "Communications",
                        ErrorCategory.NotSpecified,
                        project);
                    this.WriteError(record);
                    return;
                }
            }
        }
        #endregion
        #endregion
    }
}