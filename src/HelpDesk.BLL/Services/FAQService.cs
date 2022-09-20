using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.Common.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Services
{
    /// <inheritdoc cref="IFAQService<T>"/>
    public class FAQService : IFAQService
    {
        private readonly IRepository<FAQ> _repositoryFAQ;

        public FAQService(IRepository<FAQ> repositoryFAQ)
        {
            _repositoryFAQ = repositoryFAQ ?? throw new ArgumentNullException(nameof(repositoryFAQ));
        }

        public async Task AddFAQAsync(FAQDto fAQDto)
        {
            if (fAQDto is null)
            {
                throw new ArgumentNullException(nameof(fAQDto));
            }

            var newFAQ = new FAQ
            {
                Theme = fAQDto.Theme,
                Description = fAQDto.Description
            };

            await _repositoryFAQ.AddAsync(newFAQ);
            await _repositoryFAQ.SaveChangesAsync();
        }

        public async Task DeleteFAQAsync(int FAQid)
        {
            var FAQ = await _repositoryFAQ.GetEntityWithoutTrackingAsync(q => q.Id.Equals(FAQid));
            if (FAQ is null)
            {
                return;
            }
            _repositoryFAQ.Delete(FAQ);
            await _repositoryFAQ.SaveChangesAsync();
        }

        public async Task EditFAQAsync(FAQDto fAQDto)
        {
            if (fAQDto is null)
            {
                throw new ArgumentNullException(nameof(fAQDto));
            }

            var FAQSearch = await _repositoryFAQ.GetEntityWithoutTrackingAsync(q => q.Id.Equals(fAQDto.Id));
            if (FAQSearch is null)
            {
                return;
            }

            FAQSearch.Theme = fAQDto.Theme;
            FAQSearch.Description = fAQDto.Description;

            _repositoryFAQ.Update(FAQSearch);
            await _repositoryFAQ.SaveChangesAsync();
        }

        public async Task<List<FAQDto>> GetAllFAQAsync()
        {
            var faqDtos = new List<FAQDto>();
            var faqs = await _repositoryFAQ.GetAll().AsNoTracking().ToListAsync();

            foreach (var faq in faqs)
            {
                var faqModel = new FAQDto
                {
                    Id = faq.Id,
                    Theme = faq.Theme,
                    Description = faq.Description,
                };

                faqDtos.Add(faqModel);
            }
            return faqDtos;
        }

        public async Task<FAQDto> GetFAQByIdAsync(int id)
        {
            var faq = await _repositoryFAQ.GetEntityWithoutTrackingAsync(faq => faq.Id == id);
            if (faq is null)
            {
                return null;
            }

            var faqDto = new FAQDto
            {
                Id = faq.Id,
                Theme = faq.Theme,
                Description = faq.Description
            };
            return faqDto;
        }
    }
}
