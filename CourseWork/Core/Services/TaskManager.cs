using CourseWork.Models;
using CourseWork.Serialization;
using Services;
using System.Threading.Tasks;


namespace CourseWork.Services
{
        public class TaskManager : ITaskManager
        {
        private List<Models.Task> _tasks;
        private readonly ISerializer<Models.Task> _serializer;
        private readonly string _filePath;

        public TaskManager(ISerializer<Models.Task> serializer, string filePath)
        {
            _serializer = serializer;
            _filePath = filePath;
            _tasks = new List<Models.Task>(_serializer.Deserialize(_filePath));
            RemoveExpiredTasks(); 
        }



        public void ClearAllTasks()
        {
            _tasks.Clear();
            SaveChanges();
        }
        private void SaveChanges()
        {
            _serializer.Serialize(_tasks, _filePath);
        }

       
        public void AddTask(Models.Task task)
        {
            if (task.DueDate < DateTime.Now)
                throw new ArgumentException("Нельзя добавить задачу с прошедшей датой.");
            _tasks.Add(task);
            SaveChanges();
        }
       

          

            public void AddExam(Exam exam)
            {
            if (exam.DueDate < DateTime.Now)
                throw new ArgumentException("Нельзя добавить экзамен с прошедшей датой.");
            _tasks.Add(exam);
                 SaveChanges();
            }

            public IEnumerable<Models.Task> GetAllTasks()
            {
                return _tasks;
            }

            public IEnumerable<Exam> GetAllExams()
            {
                return _tasks.OfType<Exam>();
            }

            public Models.Task GetTaskById(Guid id)
            {
                return _tasks.FirstOrDefault(t => t.Id == id);
            }

            public void MarkTaskAsCompleted(Guid id)
            {
                var task = GetTaskById(id);
                if (task != null)
                {
                    task.MarkAsCompleted();
                }
                SaveChanges();
            }

            public void RemoveTask(Guid id)
            {
                var task = GetTaskById(id);
                if (task != null)
                {
                    _tasks.Remove(task);
                }
               SaveChanges();
            }
        public void RemoveExpiredTasks()
        {
            var now = DateTime.Now;

            _tasks.RemoveAll(t =>
                t.DueDate < now || t.Completed
            );

            SaveChanges();
        }


        /*    public void AddGoogleCalendarEvent(GoogleCalendarEvent calendarEvent)
            {
                var task = new CourseWork.Models.Task(
                    title: calendarEvent.Summary,
                    description: calendarEvent.Description ?? "Событие из Google Calendar",
                    dueDate: calendarEvent.Start,
                    relatedToLesson: false
                );

                _tasks.Add(task);
            }*/
    }

}