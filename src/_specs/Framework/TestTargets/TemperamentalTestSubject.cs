#region New BSD License

// Copyright (c) 2012, John Batte
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted
// provided that the following conditions are met:
// 
// Redistributions of source code must retain the above copyright notice, this list of conditions
// and the following disclaimer.
// 
// Redistributions in binary form must reproduce the above copyright notice, this list of conditions
// and the following disclaimer in the documentation and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Linq;
using System.Reactive.Subjects;

using _specs.Properties;
using _specs.Steps.State;

namespace _specs.Framework.TestTargets
{
	public class TemperamentalTestSubject
	{
		public const string DefaultPropertyValue = "Patterns.Specifications.TemperamentalTestSubject.DefaultPropertyValue";
		public static readonly Type TestPropertyType = typeof (string);
		public static readonly Type TestMethodReturnType = typeof (string);

		private readonly Subject<TestEvent> _callRequests = new Subject<TestEvent>();
		private readonly Subject<TestEvent> _callResponses = new Subject<TestEvent>();
		private readonly Subject<TestEvent> _readRequests = new Subject<TestEvent>();
		private readonly Subject<TestEvent> _readResponses = new Subject<TestEvent>();
		private readonly Subject<TestEvent> _writeRequests = new Subject<TestEvent>();
		private string _propertyValue;

		public TemperamentalTestSubject()
		{
			_propertyValue = DefaultPropertyValue;
		}

		/// <summary>
		/// 	Gets the call requests.
		/// </summary>
		public IObservable<TestEvent> CallRequests
		{
			get { return _callRequests; }
		}

		/// <summary>
		/// 	Gets the call responses.
		/// </summary>
		public IObservable<TestEvent> CallResponses
		{
			get { return _callResponses; }
		}

		/// <summary>
		/// 	Gets the read requests.
		/// </summary>
		public IObservable<TestEvent> ReadRequests
		{
			get { return _readRequests; }
		}

		/// <summary>
		/// 	Gets the read responses.
		/// </summary>
		public IObservable<TestEvent> ReadResponses
		{
			get { return _readResponses; }
		}

		/// <summary>
		/// 	Gets the write requests.
		/// </summary>
		public IObservable<TestEvent> WriteRequests
		{
			get { return _writeRequests; }
		}

		public string Property
		{
			get
			{
				PublishTestEvent(_readRequests, new TestEvent());
				string value = _propertyValue;
				PublishTestEvent(_readResponses, new TestEvent());
				return value;
			}
			set
			{
				PublishTestEvent(_writeRequests, new TestEvent());
				_propertyValue = value;
			}
		}

		public string AngryProperty
		{
			get
			{
				PublishTestEvent(_readRequests, new TestEvent());
				throw new Exception(Resources.PropertyReadBackTalk);
			}
			set
			{
				PublishTestEvent(_writeRequests, new TestEvent());
				throw new Exception(Resources.PropertyWriteBackTalk);
				_propertyValue = value;
			}
		}

		public string ReverseText(string text)
		{
			PublishTestEvent(_callRequests, new TestEvent());
			var value = new string(text.Reverse().ToArray());
			PublishTestEvent(_callResponses, new TestEvent());
			return value;
		}

		public string AngryReverseText(string text)
		{
			PublishTestEvent(_callRequests, new TestEvent());
			throw new Exception(Resources.MethodCallBackTalk);
		}

		private static void PublishTestEvent(IObserver<TestEvent> observer, TestEvent data)
		{
			try
			{
				observer.OnNext(data);
			}
			catch (Exception ex)
			{
				observer.OnError(ex);
			}
		}
	}
}