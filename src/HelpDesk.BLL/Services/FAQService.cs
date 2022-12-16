using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.Common.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Services
{
    /// <inheritdoc cref="IFAQService<T>"/>
    public class FAQService : IFAQService
    {
        private readonly IRepository<FAQ> _repositoryFAQ;
        private readonly IRepository<FAQTopic> _repositoryFAQTopic;

        public FAQService(IRepository<FAQ> repositoryFAQ, IRepository<FAQTopic> repositoryFAQTopic)
        {
            _repositoryFAQ = repositoryFAQ ?? throw new ArgumentNullException(nameof(repositoryFAQ));
            _repositoryFAQTopic = repositoryFAQTopic ?? throw new ArgumentNullException(nameof(repositoryFAQTopic));
        }

        public async Task AddFaqTopicAsync(FAQTopicDto fAQTopicDto) {
           
            if (fAQTopicDto is null)
            {
                throw new ArgumentNullException(nameof(fAQTopicDto));
            }

            var newFAQTopic = new FAQTopic
            {
                Theme = fAQTopicDto.Topic
            };

            await _repositoryFAQTopic.AddAsync(newFAQTopic);
            await _repositoryFAQTopic.SaveChangesAsync();
        }

        public async Task EditFAQTopicAsync(FAQTopicDto fAQTopicDto)
        {
            if (fAQTopicDto is null)
            {
                throw new ArgumentNullException(nameof(fAQTopicDto));
            }

            var FAQTopicSearch = await _repositoryFAQTopic.GetEntityWithoutTrackingAsync(q => q.Id.Equals(fAQTopicDto.Id));
            if (FAQTopicSearch is null)
            {
                return;
            }

            FAQTopicSearch.Theme = fAQTopicDto.Topic;


            _repositoryFAQTopic.Update(FAQTopicSearch);
            await _repositoryFAQTopic.SaveChangesAsync();
        }

        public async Task<FAQTopicDto> GetFAQTopicByIdAsync(int id)
        {
            var faqtopic = await _repositoryFAQTopic.GetEntityWithoutTrackingAsync(faqtopic => faqtopic.Id == id);
            if (faqtopic is null)
            {
                return null;
            }

            var faqTopicDto = new FAQTopicDto
            {
                Id = faqtopic.Id,
                Topic = faqtopic.Theme
            };
            return faqTopicDto;
        }

        public async Task<List<FAQTopicDto>> GetAllFAQTopicAsync()
        {
            var faqTopicDtos = new List<FAQTopicDto>();
            var faqTopics = await _repositoryFAQTopic.GetAll().AsNoTracking().ToListAsync();

            foreach (var faqTopic in faqTopics)
            {
                var faqTopicModel = new FAQTopicDto
                {
                    Id = faqTopic.Id,
                    Topic = faqTopic.Theme
                };

                faqTopicDtos.Add(faqTopicModel);
            }
            return faqTopicDtos;
        }

        public async Task<bool> DeleteFAQTopicAsync(FAQTopicDto fAQTopicDto)
        {
            if (fAQTopicDto is null)
            {
                throw new ArgumentNullException(nameof(fAQTopicDto));
            }
            var faqs = await _repositoryFAQ
                .GetAll()
                .AsNoTracking()
                .Where(faqs => faqs.FAQTopicId == fAQTopicDto.Id)
                .ToListAsync();

            if (!faqs.Any())
            {
                var faqTopic = await _repositoryFAQTopic.GetEntityAsync(topic => topic.Id == fAQTopicDto.Id);
                _repositoryFAQTopic.Delete(faqTopic);
                await _repositoryFAQTopic.SaveChangesAsync();
                return true;
            }
            return false;
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
                Description = fAQDto.Description,
                FAQTopicId = fAQDto.FAQTopicId
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
                    FAQTopicId = faq.FAQTopicId
                };

                faqDtos.Add(faqModel);
            }
            return faqDtos;
        }

        public async Task<List<FAQDto>> GetFAQByTopicAsync(int fAQTopicId)
        {
            var faqDtos = new List<FAQDto>();

            var faqs = await _repositoryFAQ
                .GetAll()
                .AsNoTracking()
                .Where(faq => faq.FAQTopicId == fAQTopicId)
                .ToListAsync();

            if (faqs is null)
            {
                throw new ArgumentNullException(nameof(faqs));
            }

            foreach (var faq in faqs)
            {
                var faqModel = new FAQDto
                {
                    Id = faq.Id,
                    Theme = faq.Theme,
                    Description = faq.Description,
                    FAQTopicId = faq.FAQTopicId
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
                Description = faq.Description,
                FAQTopicId = faq.FAQTopicId
            };
            return faqDto;
        }
    }
}
