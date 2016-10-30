using System.Collections.Generic;
using System.Linq;

namespace System.Threading.Tasks
{
    public static class TaskEx
    {
        public static async Task<T> WaitAny<T>(Func<T, bool> Predicate, ICollection<Task<T>> Tasks)
        {
            if (!(Tasks is List<Task<T>>))
                Tasks = Tasks.ToList();
            while (Tasks.Count() > 0)
            {
                var finishedTask = await Task.WhenAny(Tasks);
                Tasks.Remove(finishedTask);
                if (Predicate(await finishedTask))
                    return await finishedTask;
            }
            return default(T);
        }

        public static Task<T> WaitAny<T>(Func<T, bool> Predicate, params Task<T>[] Tasks) => WaitAny(Predicate, Tasks as ICollection<Task<T>>);

        public static async Task<Task<T>> WhenAny<T>(Func<T, bool> Predicate, ICollection<Task<T>> Tasks)
        {
            if (!(Tasks is List<Task<T>>))
                Tasks = Tasks.ToList();
            while (Tasks.Count() > 0)
            {
                var finishedTask = await Task.WhenAny(Tasks);
                Tasks.Remove(finishedTask);
                if (Predicate(await finishedTask))
                    return finishedTask;
            }
            return null;
        }

        public static Task<Task<T>> WhenAny<T>(Func<T, bool> Predicate, params Task<T>[] Tasks) => WhenAny(Predicate, Tasks as ICollection<Task<T>>);
    }
}
