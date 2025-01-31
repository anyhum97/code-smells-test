using MP.Domain.Model;
using MP.Model.WS;

namespace MP.WS.Windsor.Adapter
{
    public class TicketAdapter: 
        IAdapter<Ticket, TicketViewModel>,
        IAdapter<TicketSaveModel, Ticket>
    {
        public TicketViewModel Convert(Ticket domainModel)
        {
            return new TicketViewModel()
            {
                ID = domainModel.ID,
                Title = domainModel.Title,
                Description = domainModel.Description,
                VisitDate = domainModel.VisitDate,
                VisitorsNumber = domainModel.VisitorsNumber,
                CreationDate = domainModel.CreationDate
            };
        }

        public Ticket Convert(TicketSaveModel model)
        {
            return new Ticket()
            {
                Title = model.Title,
                Description = model.Description,
                VisitDate = model.VisitDate,
                VisitorsNumber = model.VisitorsNumber
            };
        }
    }
}