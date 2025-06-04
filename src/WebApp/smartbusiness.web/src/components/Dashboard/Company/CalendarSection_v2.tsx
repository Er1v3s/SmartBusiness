// // @ts-expect-error: toast-ui/calendar has incomplete or broken types, safe to ignore for now
// import Calendar from "@toast-ui/calendar";
// import "@toast-ui/calendar/dist/toastui-calendar.min.css";
// import { useEffect, useRef, useState } from "react";
// import {
//   Calendar as CalendarIcon,
//   ChevronLeft,
//   ChevronRight,
//   CalendarDays,
//   Plus,
//   X as CloseIcon,
// } from "lucide-react";
// import "./calendar-hide-headers.css";

// // Rozszerz typy eventów i modalEvent o assignee i bgColor

// type EventModal = {
//   id: string;
//   title: string;
//   start: string;
//   end: string;
//   assignee?: string;
//   bgColor?: string;
// };

// interface CalendarEvent {
//   id: string;
//   calendarId: string;
//   title: string;
//   category: string;
//   start: string;
//   end: string;
//   assignee?: string;
//   bgColor?: string;
// }

// // Pomocniczy typ do obsługi Toast UI Calendar bez typów
// interface ToastCalendar {
//   clear: () => void;
//   createEvents: (evs: unknown[]) => void;
//   on?: (event: string, cb: (e: unknown) => void) => void;
//   destroy?: () => void;
// }

// // Przykładowe eventy jako stałe (mock)

// const MOCK_EVENTS: CalendarEvent[] = [
//   {
//     id: "1",
//     calendarId: "cal1",
//     title: "Lunch",
//     category: "time",
//     start: "2025-06-04T12:00:00",
//     end: "2025-06-04T13:30:00",
//     assignee: "Jan Kowalski",
//     bgColor: "#6366f1",
//   },
//   {
//     id: "2",
//     calendarId: "cal1",
//     title: "Coffee Break",
//     category: "time",
//     start: "2025-06-04T15:00:00",
//     end: "2025-06-04T15:30:00",
//     assignee: "Anna Nowak",
//     bgColor: "#f59e42",
//   },
//   {
//     id: "3",
//     calendarId: "cal1",
//     title: "Team Meeting",
//     category: "time",
//     start: "2025-06-05T09:00:00",
//     end: "2025-06-05T10:00:00",
//     assignee: "Zespół",
//     bgColor: "#10b981",
//   },
//   {
//     id: "4",
//     calendarId: "cal1",
//     title: "Project Review",
//     category: "time",
//     start: "2025-06-06T14:00:00",
//     end: "2025-06-06T15:00:00",
//     assignee: "Marta Zielińska",
//     bgColor: "#ef4444",
//   },
// ];

// export const CalendarSection = () => {
//   const calendarRef = useRef<HTMLDivElement>(null);
//   const calendarInstance = useRef<unknown>(null);
//   const [view, setView] = useState<"month" | "week" | "day">("month");
//   const [modalEvent, setModalEvent] = useState<EventModal | null>(null);
//   const [events, setEvents] = useState(MOCK_EVENTS);
//   const [showAddModal, setShowAddModal] = useState(false);
//   const [addForm, setAddForm] = useState({
//     title: "",
//     start: "",
//     end: "",
//     assignee: "",
//     color: "#6366f1", // domyślny indigo
//   });
//   const [addFormError, setAddFormError] = useState<string | null>(null);

//   useEffect(() => {
//     if (calendarRef.current) {
//       calendarInstance.current = new Calendar(calendarRef.current, {
//         defaultView: view,
//         usageStatistics: false,
//         height: "800px",
//         calendars: [{ id: "cal1", name: "Personal" }],
//         events: [],
//         month: {
//           dayNames: ["S", "M", "T", "W", "T", "F", "S"],
//           visibleWeeksCount: 3,
//         },
//       });
//       // Obsługa kliknięcia eventu
//       (calendarInstance.current as ToastCalendar).on?.(
//         "clickEvent",
//         (e: unknown) => {
//           if (e && typeof e === "object" && "event" in e) {
//             const eventObj = (e as { event: unknown }).event as EventModal;
//             setModalEvent({
//               id: eventObj.id,
//               title: eventObj.title,
//               start: eventObj.start,
//               end: eventObj.end,
//               assignee: (eventObj as CalendarEvent).assignee,
//               bgColor: (eventObj as CalendarEvent).bgColor,
//             });
//           }
//         },
//       );
//       // Dodaj eventy po inicjalizacji
//       (calendarInstance.current as ToastCalendar).createEvents(events);
//     }
//     return () => {
//       if (
//         calendarInstance.current &&
//         typeof (calendarInstance.current as ToastCalendar).destroy ===
//           "function"
//       ) {
//         (calendarInstance.current as ToastCalendar).destroy!();
//       }
//     };
//   }, [view, events]);

//   // Synchronizuj eventy po zmianie events
//   useEffect(() => {
//     const inst = calendarInstance.current as ToastCalendar;
//     if (
//       inst &&
//       typeof inst.clear === "function" &&
//       typeof inst.createEvents === "function"
//     ) {
//       inst.clear();
//       inst.createEvents(events);
//     }
//   }, [events]);

//   // Dodawanie eventu (przykład: losowy event na dziś)
//   const handleAddEvent = () => {
//     setShowAddModal(true);
//     setAddForm({
//       title: "",
//       start: "",
//       end: "",
//       assignee: "",
//       color: "#6366f1",
//     });
//     setAddFormError(null);
//   };

//   const handleAddFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
//     setAddForm({ ...addForm, [e.target.name]: e.target.value });
//   };

//   const handleAddFormSubmit = (e: React.FormEvent) => {
//     e.preventDefault();
//     if (
//       !addForm.title.trim() ||
//       !addForm.start ||
//       !addForm.end ||
//       !addForm.assignee.trim()
//     ) {
//       setAddFormError("All fields are required.");
//       return;
//     }
//     if (new Date(addForm.end) <= new Date(addForm.start)) {
//       setAddFormError("End date must be after start date.");
//       return;
//     }
//     setEvents([
//       ...events,
//       {
//         id: (events.length + 1).toString(),
//         calendarId: "cal1",
//         title: addForm.title,
//         category: "time",
//         start: new Date(addForm.start).toISOString(),
//         end: new Date(addForm.end).toISOString(),
//         assignee: addForm.assignee,
//         bgColor: addForm.color,
//       },
//     ]);
//     setShowAddModal(false);
//   };

//   // Modal zamykanie ESC i klik poza
//   useEffect(() => {
//     const onKeyDown = (e: KeyboardEvent) => {
//       if (e.key === "Escape") setModalEvent(null);
//     };
//     if (modalEvent) window.addEventListener("keydown", onKeyDown);
//     return () => window.removeEventListener("keydown", onKeyDown);
//   }, [modalEvent]);

//   // Navigation handlers
//   const handlePrev = () => {
//     if (
//       calendarInstance.current &&
//       typeof (calendarInstance.current as { prev?: () => void }).prev ===
//         "function"
//     ) {
//       (calendarInstance.current as { prev: () => void }).prev();
//     }
//   };
//   const handleNext = () => {
//     if (
//       calendarInstance.current &&
//       typeof (calendarInstance.current as { next?: () => void }).next ===
//         "function"
//     ) {
//       (calendarInstance.current as { next: () => void }).next();
//     }
//   };
//   const handleToday = () => {
//     if (
//       calendarInstance.current &&
//       typeof (calendarInstance.current as { today?: () => void }).today ===
//         "function"
//     ) {
//       (calendarInstance.current as { today: () => void }).today();
//     }
//   };
//   const handleViewChange = (v: "month" | "week" | "day") => {
//     setView(v);
//   };

//   return (
//     <div className="flex h-[calc(100vh-(var(--spacing)*16))] bg-gradient-to-br from-gray-100 to-gray-300 dark:from-gray-800 dark:to-gray-700">
//       <div className="flex flex-1 items-start justify-center bg-transparent px-4 py-12 text-gray-700 dark:text-gray-100">
//         <div className="w-full max-w-3xl">
//           <div className="mb-4 flex items-center justify-between gap-4">
//             <div className="flex items-center gap-2 text-indigo-700 dark:text-indigo-300">
//               <CalendarIcon className="h-6 w-6" />
//               <h1 className="text-2xl font-bold">Calendar</h1>
//             </div>
//             <div className="flex gap-2">
//               <button
//                 onClick={handlePrev}
//                 className="rounded-lg border border-gray-200 bg-white px-2 py-1 text-gray-700 shadow hover:bg-gray-100 dark:border-gray-800 dark:bg-gray-900 dark:text-gray-100 dark:hover:bg-gray-800"
//                 title="Previous"
//               >
//                 <ChevronLeft className="h-5 w-5" />
//               </button>
//               <button
//                 onClick={handleToday}
//                 className="rounded-lg border border-gray-200 bg-white px-2 py-1 text-gray-700 shadow hover:bg-gray-100 dark:border-gray-800 dark:bg-gray-900 dark:text-gray-100 dark:hover:bg-gray-800"
//                 title="Today"
//               >
//                 <CalendarDays className="h-5 w-5" />
//               </button>
//               <button
//                 onClick={handleNext}
//                 className="rounded-lg border border-gray-200 bg-white px-2 py-1 text-gray-700 shadow hover:bg-gray-100 dark:border-gray-800 dark:bg-gray-900 dark:text-gray-100 dark:hover:bg-gray-800"
//                 title="Next"
//               >
//                 <ChevronRight className="h-5 w-5" />
//               </button>
//             </div>
//             <div className="flex gap-2">
//               <button
//                 onClick={() => handleViewChange("month")}
//                 className={`rounded-lg px-3 py-1 text-sm font-medium shadow transition-colors ${
//                   view === "month"
//                     ? "bg-indigo-600 text-white"
//                     : "bg-white text-gray-700 dark:bg-gray-900 dark:text-gray-100"
//                 }`}
//               >
//                 Month
//               </button>
//               <button
//                 onClick={() => handleViewChange("week")}
//                 className={`rounded-lg px-3 py-1 text-sm font-medium shadow transition-colors ${
//                   view === "week"
//                     ? "bg-indigo-600 text-white"
//                     : "bg-white text-gray-700 dark:bg-gray-900 dark:text-gray-100"
//                 }`}
//               >
//                 Week
//               </button>
//               <button
//                 onClick={() => handleViewChange("day")}
//                 className={`rounded-lg px-3 py-1 text-sm font-medium shadow transition-colors ${
//                   view === "day"
//                     ? "bg-indigo-600 text-white"
//                     : "bg-white text-gray-700 dark:bg-gray-900 dark:text-gray-100"
//                 }`}
//               >
//                 Day
//               </button>
//             </div>
//             <button
//               onClick={handleAddEvent}
//               className="ml-2 flex items-center gap-1 rounded-lg bg-indigo-600 px-3 py-1 text-sm font-semibold text-white shadow transition hover:bg-indigo-700"
//               title="Add event"
//             >
//               <Plus className="h-4 w-4" /> Add
//             </button>
//           </div>
//           <div
//             ref={calendarRef}
//             className="min-h-[600px] max-w-full min-w-[340px] rounded-xl border border-gray-200 bg-white shadow-xl dark:border-gray-900 dark:bg-gray-800"
//             style={{ height: 600 }}
//           />
//           {modalEvent && (
//             <div
//               className="fixed inset-0 z-50 flex items-center justify-center bg-black/40"
//               onClick={() => setModalEvent(null)}
//             >
//               <div
//                 className="relative w-full max-w-md rounded-xl bg-white p-6 shadow-xl dark:bg-gray-900"
//                 onClick={(e) => e.stopPropagation()}
//               >
//                 <button
//                   className="absolute top-2 right-2 rounded p-1 text-gray-400 hover:bg-gray-100 dark:hover:bg-gray-800"
//                   onClick={() => setModalEvent(null)}
//                   title="Close"
//                 >
//                   <CloseIcon className="h-5 w-5" />
//                 </button>
//                 <h2 className="mb-2 text-xl font-bold text-indigo-700 dark:text-indigo-300">
//                   {modalEvent.title}
//                 </h2>
//                 <div className="mb-2 text-gray-700 dark:text-gray-200">
//                   <b>Start:</b> {new Date(modalEvent.start).toLocaleString()}
//                   <br />
//                   <b>End:</b> {new Date(modalEvent.end).toLocaleString()}
//                   <br />
//                   {modalEvent.assignee && (
//                     <>
//                       <b>Assignee:</b> {modalEvent.assignee}
//                       <br />
//                     </>
//                   )}
//                   {modalEvent.bgColor && (
//                     <>
//                       <b>Color:</b>{" "}
//                       <span
//                         style={{
//                           background: modalEvent.bgColor,
//                           padding: "0 10px",
//                           borderRadius: 4,
//                         }}
//                       />{" "}
//                       {modalEvent.bgColor}
//                       <br />
//                     </>
//                   )}
//                 </div>
//                 <div className="text-sm text-gray-500">ID: {modalEvent.id}</div>
//               </div>
//             </div>
//           )}
//           {showAddModal && (
//             <div
//               className="fixed inset-0 z-50 flex items-center justify-center bg-black/40"
//               onClick={() => setShowAddModal(false)}
//             >
//               <form
//                 className="relative w-full max-w-md rounded-xl bg-white p-6 shadow-xl dark:bg-gray-900"
//                 onClick={(e) => e.stopPropagation()}
//                 onSubmit={handleAddFormSubmit}
//               >
//                 <button
//                   className="absolute top-2 right-2 rounded p-1 text-gray-400 hover:bg-gray-100 dark:hover:bg-gray-800"
//                   onClick={() => setShowAddModal(false)}
//                   type="button"
//                   title="Close"
//                 >
//                   <CloseIcon className="h-5 w-5" />
//                 </button>
//                 <h2 className="mb-4 text-xl font-bold text-indigo-700 dark:text-indigo-300">
//                   Add Event
//                 </h2>
//                 <div className="mb-3">
//                   <label className="mb-1 block text-sm font-medium">
//                     Title
//                   </label>
//                   <input
//                     name="title"
//                     type="text"
//                     className="w-full rounded border border-gray-300 px-3 py-2 text-gray-900 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-100"
//                     value={addForm.title}
//                     onChange={handleAddFormChange}
//                     required
//                   />
//                 </div>
//                 <div className="mb-3">
//                   <label className="mb-1 block text-sm font-medium">
//                     Start
//                   </label>
//                   <input
//                     name="start"
//                     type="datetime-local"
//                     className="w-full rounded border border-gray-300 px-3 py-2 text-gray-900 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-100"
//                     value={addForm.start}
//                     onChange={handleAddFormChange}
//                     required
//                   />
//                 </div>
//                 <div className="mb-3">
//                   <label className="mb-1 block text-sm font-medium">End</label>
//                   <input
//                     name="end"
//                     type="datetime-local"
//                     className="w-full rounded border border-gray-300 px-3 py-2 text-gray-900 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-100"
//                     value={addForm.end}
//                     onChange={handleAddFormChange}
//                     required
//                   />
//                 </div>
//                 <div className="mb-3">
//                   <label className="mb-1 block text-sm font-medium">
//                     Assignee
//                   </label>
//                   <input
//                     name="assignee"
//                     type="text"
//                     className="w-full rounded border border-gray-300 px-3 py-2 text-gray-900 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-100"
//                     value={addForm.assignee}
//                     onChange={handleAddFormChange}
//                     required
//                   />
//                 </div>
//                 <div className="mb-3">
//                   <label className="mb-1 block text-sm font-medium">
//                     Color
//                   </label>
//                   <input
//                     name="color"
//                     type="color"
//                     className="h-10 w-16 cursor-pointer rounded border border-gray-300 bg-transparent p-0 dark:border-gray-700"
//                     value={addForm.color}
//                     onChange={handleAddFormChange}
//                     required
//                   />
//                 </div>
//                 {addFormError && (
//                   <div className="mb-2 text-sm text-red-500">
//                     {addFormError}
//                   </div>
//                 )}
//                 <div className="flex justify-end gap-2">
//                   <button
//                     type="button"
//                     className="rounded bg-gray-200 px-4 py-2 text-gray-700 hover:bg-gray-300 dark:bg-gray-800 dark:text-gray-100 dark:hover:bg-gray-700"
//                     onClick={() => setShowAddModal(false)}
//                   >
//                     Cancel
//                   </button>
//                   <button
//                     type="submit"
//                     className="rounded bg-indigo-600 px-4 py-2 font-semibold text-white hover:bg-indigo-700"
//                   >
//                     Add
//                   </button>
//                 </div>
//               </form>
//             </div>
//           )}
//         </div>
//       </div>
//     </div>
//   );
// };
