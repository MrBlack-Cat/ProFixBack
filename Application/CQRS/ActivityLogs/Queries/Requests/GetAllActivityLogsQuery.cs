﻿using Application.CQRS.ActivityLogs.DTOs;
using Common.GlobalResponse;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.ActivityLogs.Queries.Requests;

public record GetAllActivityLogsQuery : IRequest<ResponseModel<List<ActivityLogListDto>>>;
