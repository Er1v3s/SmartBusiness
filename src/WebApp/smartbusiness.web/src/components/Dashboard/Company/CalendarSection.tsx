// @ts-expect-error: toast-ui/calendar has incomplete or broken types, safe to ignore for now
import Calendar from "@toast-ui/calendar";
import "@toast-ui/calendar/dist/toastui-calendar.min.css";
import { useEffect, useRef, useState } from "react";
import { CalendarViewButton } from "./Calendar/CalendarViewButton";
import { ChevronLeft, ChevronRight, CalendarDays } from "lucide-react";

interface CalendarEvent {
  id: string;
  calendarId: string;
  title: string;
  category: string;
  dueDateClass?: string;
  start: string;
  end: string;
}

const events: CalendarEvent[] = [
  {
    id: "1",
    calendarId: "cal1",
    title: "Makijaż permanentny ust",
    category: "time",
    start: "2025-06-14T11:00:00",
    end: "2025-06-14T14:00:00",
  },
  {
    id: "1",
    calendarId: "cal1",
    title: "Peeling węglowy",
    category: "time",
    start: "2025-06-14T14:05:00",
    end: "2025-06-14T14:35:00",
  },
  {
    id: "1",
    calendarId: "cal1",
    title: "Lipoliza",
    category: "time",
    start: "2025-06-14T15:00:00",
    end: "2025-06-14T15:30:00",
  },
  {
    id: "1",
    calendarId: "cal1",
    title: "Oczyszczanie manulane skóry",
    category: "time",
    start: "2025-06-14T15:35:00",
    end: "2025-06-14T16:35:00",
  },
];

export const CalendarSection = () => {
  const calendarRef = useRef<HTMLDivElement>(null);
  const [view, setView] = useState<"month" | "week" | "day">("week");
  const calendarInstance = useRef<Calendar>(null);

  useEffect(() => {
    if (!calendarRef.current) return;
    const calendar = new Calendar(calendarRef.current, {
      defaultView: view,
      useCreationPopup: true,
      useDetailPopup: true,
      events: [],
      week: {
        hourStart: 6,
        hourEnd: 23,
        eventView: true,
        taskView: false,
      },
    });
    calendar.createEvents(events);
    calendarInstance.current = calendar;
    return () => {
      calendar.destroy?.();
      calendarInstance.current = null;
    };
  }, [view]);

  // Handlers for navigation buttons
  const handlePrev = () => {
    calendarInstance.current?.prev?.();
  };
  const handleNext = () => {
    calendarInstance.current?.next?.();
  };
  const handleToday = () => {
    calendarInstance.current?.today?.();
  };

  const [now, setNow] = useState(() => new Date());
  useEffect(() => {
    const interval = setInterval(() => {
      setNow(new Date());
    }, 60000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="h-full w-full">
      <div className="mb-4 flex items-center justify-between gap-2">
        {/* View switch buttons on the left */}
        <div className="flex gap-2">
          <CalendarViewButton
            active={view === "month"}
            onClick={() => setView("month")}
          >
            Month
          </CalendarViewButton>
          <CalendarViewButton
            active={view === "week"}
            onClick={() => setView("week")}
          >
            Week
          </CalendarViewButton>
          <CalendarViewButton
            active={view === "day"}
            onClick={() => setView("day")}
          >
            Day
          </CalendarViewButton>
        </div>

        {/* Centered navigation buttons */}
        <div className="flex justify-center gap-2">
          <button
            className="rounded-md border border-gray-200 bg-white px-2 py-1 text-indigo-700 shadow hover:bg-indigo-50 dark:border-gray-700 dark:bg-gray-800 dark:text-indigo-200 dark:hover:bg-gray-900"
            onClick={handlePrev}
            title="Poprzedni zakres"
          >
            <ChevronLeft className="h-5 w-5" />
          </button>
          <button
            className="rounded-md border border-gray-200 bg-white px-2 py-1 text-indigo-700 shadow hover:bg-indigo-50 dark:border-gray-700 dark:bg-gray-800 dark:text-indigo-200 dark:hover:bg-gray-900"
            onClick={handleToday}
            title="Dzisiaj"
          >
            <CalendarDays className="h-5 w-5" />
          </button>
          <button
            className="rounded-md border border-gray-200 bg-white px-2 py-1 text-indigo-700 shadow hover:bg-indigo-50 dark:border-gray-700 dark:bg-gray-800 dark:text-indigo-200 dark:hover:bg-gray-900"
            onClick={handleNext}
            title="Następny zakres"
          >
            <ChevronRight className="h-5 w-5" />
          </button>
        </div>

        {/* Timer */}
        <div className="flex items-center justify-end">
          <div className="flex items-center gap-2 px-2 py-0.5 font-mono text-base text-gray-700 select-none md:text-lg dark:text-gray-200">
            <span className="font-semibold">
              {now.toLocaleDateString("pl-PL", {
                weekday: "long",
                day: "2-digit",
                month: "2-digit",
                year: "numeric",
              })}
            </span>
            <span className="h-2 w-2 animate-pulse rounded-full bg-green-400 dark:bg-green-300"></span>
            <span className="tracking-tight tabular-nums">
              {now.toLocaleTimeString("pl-PL", {
                hour: "2-digit",
                minute: "2-digit",
              })}
            </span>
          </div>
        </div>
      </div>

      {/* Calendar container */}
      <div
        ref={calendarRef}
        className="h-full w-full"
        style={{ height: "90%", width: "100%" }}
      ></div>
    </div>
  );
};
