using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.Linq.Expressions;
using System.Threading;

namespace IssacLike.Source.Util {
    internal static class Coroutine {

        private static Dictionary<string, CancellationTokenSource> m_CoroutineTask = new Dictionary<string, CancellationTokenSource>();

        public static void StartCoroutine(Expression<Func<IEnumerator>> factory) {
            var body = (MethodCallExpression)factory.Body;
            var methodName = body.Method.Name;

            var cancel = new CancellationTokenSource();
            Task task = Task.Run(() => RunCoroutine(factory, cancel.Token, cancel), cancel.Token);
            
            if(!m_CoroutineTask.ContainsKey(methodName)) m_CoroutineTask.Add(methodName, cancel);
        }

        private static async Task RunCoroutine(Expression<Func<IEnumerator>> factory, CancellationToken token, CancellationTokenSource source) {
         
            while (!token.IsCancellationRequested) {
                IEnumerator coroutine = factory.Compile().Invoke();

                while (coroutine.MoveNext()) {

                    await Task.Yield();

                }
            }                                     
        }

        public static void StopCoroutine(string coroutine) {
            if (m_CoroutineTask.TryGetValue(coroutine, out CancellationTokenSource token)) {
                Debug.WriteLine($"Stopping task with {token}");
                token.Cancel();
                m_CoroutineTask.Remove(coroutine);
            }
        }
    }

    internal class WaitForSeconds {
        private float m_Duration;
        private Stopwatch m_StopWatch;

        public WaitForSeconds(float duration) {
            m_Duration = duration;
            m_StopWatch = new Stopwatch();
            m_StopWatch.Start();
        }

        public bool IsWaitFinished() {
            return m_StopWatch.Elapsed.TotalSeconds >= m_Duration;
        }
    }
}
